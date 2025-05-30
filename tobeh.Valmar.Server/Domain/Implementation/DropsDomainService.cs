using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;
using tobeh.Valmar.Server.Util;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class DropsDomainService(
    ILogger<DropsDomainService> logger,
    PalantirContext db
) : IDropsDomainService
{
    public async Task ScheduleDrop(int delaySeconds, int? eventDropId)
    {
        logger.LogTrace("ScheduleDrop(delaySeconds={delaySeconds}, eventDropId={eventDropId})", delaySeconds,
            eventDropId);

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        db.CurrrentDrops.RemoveRange(db.CurrrentDrops);
        db.CurrrentDrops.Add(new CurrrentDropEntity()
        {
            Id = timestamp,
            Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).AddSeconds(delaySeconds)
                .ToUnixTimeMilliseconds(),
            EventDropId = eventDropId,
            Cleared = false,
            Claimed = false
        });

        await db.SaveChangesAsync();
    }

    public async Task<CurrrentDropEntity> GetScheduledDrop()
    {
        logger.LogTrace("GetScheduledDrop()");
        return await db.CurrrentDrops.FirstAsync();
    }

    public Task<Tuple<int, int>> CalculateDropDelayBounds(int playerCount, double boostFactor)
    {
        logger.LogTrace("CalculateDropDelayBounds(playerCount={playerCount}, boostFactor={boostFactor})", playerCount,
            boostFactor);

        if (playerCount <= 0) playerCount = 1;
        int min = 600 / playerCount;
        if (min < 30) min = 30;
        min += 20; // minimum offset

        // modify by boosts
        var boostModifiedOffset =
            min -
            min / Math.Pow(0.8 * Math.E, 0.5) +
            min / Math.Pow(0.8 * Math.E, boostFactor / 2);

        min = Convert.ToInt32(Math.Round(boostModifiedOffset, 0));

        return Task.FromResult(new Tuple<int, int>(min, 4 * min));
    }

    public async Task<double> GetCurrentDropBoost()
    {
        logger.LogTrace("GetCurrentDropBoost()");

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var boost = 1 + await db.DropBoosts
            .Where(b => Convert.ToInt32(b.StartUtcs) + b.DurationS > timestamp)
            .Select(b => Convert.ToDouble(b.Factor) - 1)
            .SumAsync();

        return boost;
    }

    public async Task<ClaimDropResultDdo> ClaimDrop(long dropId, bool leagueMode)
    {
        logger.LogTrace("ClaimDrop(dropId={dropId})", dropId);

        var now = DateTimeOffset.UtcNow;
        var drop = await db.CurrrentDrops.FirstAsync();
        var dropValidFrom = DateTimeOffset.FromUnixTimeMilliseconds(drop.Timestamp);
        var dropCatchTime = now - dropValidFrom;

        var firstClaim = false;
        var clearedDrop = false;

        if (dropCatchTime > TimeSpan.FromSeconds(3))
        {
            throw new UserOperationException("The drop has timed out");
        }

        if (dropCatchTime < TimeSpan.FromMilliseconds(187))
        {
            throw new UserOperationException("The claim has been rejected as spam");
        }

        if (drop.Cleared)
        {
            throw new UserOperationException("The drop has already been cleared by someone else");
        }

        if (drop.Id != dropId)
        {
            throw new UserOperationException("The drop claim is invalid");
        }

        /* save the drop as claimed/cleared as fast as possible - the only possible race condition */
        if (!leagueMode && !drop.Claimed)
        {
            drop.Claimed = true;
            drop.Cleared = dropCatchTime > TimeSpan.FromSeconds(1);
            db.CurrrentDrops.Update(drop);
            await db.SaveChangesAsync();
            firstClaim = true;
            clearedDrop = drop.Cleared;
        }
        else if (!leagueMode && dropCatchTime > TimeSpan.FromSeconds(1))
        {
            drop.Cleared = true;
            db.CurrrentDrops.Update(drop);
            await db.SaveChangesAsync();
            clearedDrop = true;
        }

        /* calculate drop claim result */
        var catchMs = Convert.ToInt32(dropCatchTime.TotalMilliseconds);
        var weight = DropHelper.Weight(catchMs);
        var result = new ClaimDropResultDdo(drop.Id, firstClaim, clearedDrop, catchMs, weight, leagueMode,
            drop.EventDropId);

        logger.LogInformation(
            "Finished processing drop: {dropId} (firstClaim={firstClaim}, clearedDrop={clearedDrop}, catchMs={catchMs}, weight={weight})",
            drop.Id, firstClaim, clearedDrop, catchMs, weight);
        return result;
    }

    public async Task RewardDrop(int login, int? eventDropId, double value)
    {
        logger.LogTrace("RewardDrop(login={login}, eventDropId={eventDropid}, value={value})", login, eventDropId,
            value);

        if (eventDropId is { } id)
        {
            var credit =
                await db.EventCredits.FirstOrDefaultAsync(credit => credit.EventDropId == id && credit.Login == login);
            if (credit is not null)
            {
                credit.Credit += Convert.ToSingle(value);
                db.EventCredits.Update(credit);
            }
            else
            {
                credit = new EventCreditEntity
                {
                    EventDropId = id,
                    Login = login,
                    Credit = Convert.ToSingle(value)
                };
                db.EventCredits.Add(credit);
            }

            await db.SaveChangesAsync();
        }
        else
        {
            var member = await db.Members.FirstOrDefaultAsync(m => m.Login == login);
            if (member is null)
            {
                throw new EntityNotFoundException("Member not found for login " + login);
            }

            member.Drops += value;
            db.Members.Update(member);
            await db.SaveChangesAsync();
        }
    }

    public async Task LogDropClaim(long dropId, long discordId, long claimTimestamp, string lobbyKey, int catchMs,
        int? eventDropId)
    {
        logger.LogTrace(
            "LogDropClaim(dropId={dropId}, discordId={discordId}, claimTimestamp={claimTimestamp}, lobbyKey={lobbyKey}, catchMs={catchMs})",
            dropId, discordId, claimTimestamp, lobbyKey, catchMs);

        var log = new PastDropEntity
        {
            CaughtLobbyKey = lobbyKey,
            CaughtLobbyPlayerId = discordId.ToString(),
            DropId = dropId,
            ValidFrom = DropHelper.FormatDropTimestamp(DateTimeOffset.FromUnixTimeMilliseconds(claimTimestamp)),
            LeagueWeight = catchMs,
            EventDropId = eventDropId ?? 0
        };

        db.PastDrops.Add(log);
        await db.SaveChangesAsync();
    }
}