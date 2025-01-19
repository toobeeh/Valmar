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
        var validFrom =
            DropHelper.FormatDropTimestamp(DateTimeOffset.FromUnixTimeMilliseconds(timestamp).AddSeconds(delaySeconds));

        // TODO remove this and table after typo refactor complete
        db.NextDrops.RemoveRange(db.NextDrops);
        db.NextDrops.Add(new NextDropEntity()
        {
            CaughtLobbyKey = "",
            CaughtLobbyPlayerId = "",
            DropId = timestamp,
            EventDropId = eventDropId ?? 0,
            LeagueWeight = 0,
            ValidFrom = validFrom
        });

        db.CurrrentDrops.RemoveRange(db.CurrrentDrops);
        db.CurrrentDrops.Add(new CurrrentDropEntity()
        {
            Id = timestamp,
            Timestamp = timestamp,
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

    public async Task<ClaimDropResultDdo> ClaimDrop(long dropId)
    {
        logger.LogTrace("ClaimDrop(dropId={dropId})", dropId);

        var now = DateTimeOffset.UtcNow;
        var drop = await db.CurrrentDrops.FirstAsync();
        var dropValidFrom = DateTimeOffset.FromUnixTimeMilliseconds(drop.Timestamp);
        var dropCatchTime = now - dropValidFrom;
        var validClaim = !drop.Cleared && drop.Id == dropId && dropCatchTime < TimeSpan.FromSeconds(2);
        var firstClaim = false;
        var clearedDrop = false;

        /* save the drop as claimed/cleared as fast as possible - the only possible race condition */
        if (validClaim && !drop.Claimed)
        {
            drop.Claimed = true;
            drop.Cleared = dropCatchTime > TimeSpan.FromSeconds(1);
            db.CurrrentDrops.Update(drop);
            await db.SaveChangesAsync();
            firstClaim = true;
        }
        else if (validClaim && dropCatchTime > TimeSpan.FromSeconds(1))
        {
            drop.Cleared = true;
            db.CurrrentDrops.Update(drop);
            await db.SaveChangesAsync();
            clearedDrop = true;
        }

        if (!validClaim)
        {
            throw new UserOperationException(dropCatchTime > TimeSpan.FromSeconds(2)
                ? "The drop timed out"
                : "Someone else cleared the drop");
        }

        /* calculate drop claim result */
        var catchMs = Convert.ToInt32(dropCatchTime.TotalMilliseconds);
        var weight = DropHelper.Weight(catchMs);
        var result = new ClaimDropResultDdo(drop.Id, firstClaim, clearedDrop, catchMs, weight);

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