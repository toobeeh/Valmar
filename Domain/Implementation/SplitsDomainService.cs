using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Exceptions;
using Valmar.Util;

namespace Valmar.Domain.Implementation;

public class SplitsDomainService(
    ILogger<SplitsDomainService> logger,
    IMembersDomainService membersDomainService,
    PalantirContext db) : ISplitsDomainService
{
    public async Task<List<SplitDefinitionDdo>> GetSplits()
    {
        logger.LogTrace("GetSplits()");

        var splits = await db.BoostSplits.ToListAsync();

        return splits.Select(split => new SplitDefinitionDdo(
            split.Id,
            split.Value,
            split.Name,
            split.Description,
            SplitHelper.ParseSplitTimestamp(split.Date)
        )).ToList();
    }
    
    public async Task<SplitDefinitionDdo> GetSplitById(int id)
    {
        logger.LogTrace("GetSplitById(id={id})", id);

        var split = await db.BoostSplits.FirstOrDefaultAsync(split => split.Id == id);
        if (split is null)
        {
            throw new EntityNotFoundException($"Split with ID {id} not found");
        }

        return new SplitDefinitionDdo(
            split.Id,
            split.Value,
            split.Name,
            split.Description,
            SplitHelper.ParseSplitTimestamp(split.Date)
        );
    }

    public async Task<List<SplitRewardDdo>> GetMemberSplitRewards(MemberDdo member)
    {
        logger.LogTrace("GetMemberSplitRewards(member={member})", member);
        
        var rewards = await db.SplitCredits
            .Where(reward => reward.Login == member.Login)
            .ToListAsync();

        // handle temporary splits
        if (member.MappedFlags.Contains(MemberFlagDdo.Booster))
        {
            rewards.Add(new SplitCreditEntity
            {
                Login = member.Login,
                RewardDate = SplitHelper.FormatSplitTimestamp(DateTimeOffset.UtcNow),
                Split = 31
            });
        }
        
        if (member.MappedFlags.Contains(MemberFlagDdo.Patronizer))
        {
            rewards.Add(new SplitCreditEntity
            {
                Login = member.Login,
                RewardDate = SplitHelper.FormatSplitTimestamp(DateTimeOffset.UtcNow),
                Split = 29
            });
        }
        else if (member.MappedFlags.Contains(MemberFlagDdo.Patron))
        {
            rewards.Add(new SplitCreditEntity
            {
                Login = member.Login,
                RewardDate = SplitHelper.FormatSplitTimestamp(DateTimeOffset.UtcNow),
                Split = 30
            });
        }

        var splits = await GetSplits(); // better performance as long as there is no major increase in splits
        return rewards.Select(reward =>
        {
            var rewardDate = SplitHelper.ParseSplitTimestamp(reward.RewardDate);
            var splitDefinition = splits.FirstOrDefault(split => split.Id == reward.Split) ??
                                  throw new EntityNotFoundException($"Split with ID {reward.Split} not found");

            return new SplitRewardDdo(
                reward.Login,
                splitDefinition,
                rewardDate,
                string.IsNullOrWhiteSpace(reward.Comment) ? null : reward.Comment,
                reward.ValueOverride == -1 ? null : reward.ValueOverride,
                splitDefinition.Name.Contains("League") &&
                splitDefinition.CreationDate < DateTimeOffset.UtcNow.AddMonths(-4) // league
                || (splitDefinition.Id == 20 &&
                    splitDefinition.CreationDate < DateTimeOffset.UtcNow.AddDays(-14)) // epic award
                || (splitDefinition.Id == 21 &&
                    splitDefinition.CreationDate < DateTimeOffset.UtcNow.AddDays(-28)) // legendary award
            );
        }).ToList();
    }

    public async Task RewardSplit(int rewardeeLogin, int splitId, string? comment, int? valueOverride)
    {
        logger.LogTrace("RewardSplit(rewardeeLogin={rewardeeLogin}, splitId={splitId}, comment={comment}, valueOverride={valueOverride})", rewardeeLogin, splitId, comment, valueOverride);

        // check that split exists
        await GetSplitById(splitId);
        
        // add to db
        db.SplitCredits.Add(new SplitCreditEntity
        {
            Login = rewardeeLogin,
            Split = splitId,
            RewardDate = SplitHelper.FormatSplitTimestamp(DateTimeOffset.UtcNow),
            Comment = comment ?? "",
            ValueOverride = valueOverride ?? -1
        });
        await db.SaveChangesAsync();
    }

    public async Task<List<DropboostDdo>> GetDropboosts(int? login = null)
    {
        var utcMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var activeBoosts = await db.DropBoosts
            .Where(boost => (login == null || boost.Login == login) && Convert.ToInt64(boost.StartUtcs) + boost.DurationS > utcMs)
            .ToListAsync();

        return activeBoosts.Select(boost => new DropboostDdo(
            boost.Login,
            SplitHelper.CalculateSplitCost(Convert.ToDouble(boost.Factor), boost.DurationS / 1000, boost.CooldownBonusS / 1000),
            DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(boost.StartUtcs)),
            boost.DurationS / 1000,
            Convert.ToDouble(boost.Factor),
            boost.CooldownBonusS / 1000
            )).ToList();
    }
    
    public async Task<AvailableSplitsDdo> GetAvailableSplits(MemberDdo member)
    {
        logger.LogTrace("GetAvailableSplits(member={member})", member);

        var rewards = await GetMemberSplitRewards(member);
        var boosts = await GetDropboosts(member.Login);

        var total = rewards.Where(reward => !reward.Expired).Sum(reward => reward.ValueOverride ?? reward.Split.Value);
        var used = boosts.Sum(boost => boost.Value);
        return new AvailableSplitsDdo(total, total - used);
    }

    public async Task StartDropboost(MemberDdo member, int factorSplits, int durationSplits, int cooldownSplits)
    {
        logger.LogTrace("StartDropboost(member={member}, factorSplits={factorSplits}, durationSplits={durationSplits}, cooldownSplits={cooldownSplits})", member, factorSplits, durationSplits, cooldownSplits);
        
        var availableSplits = await GetAvailableSplits(member);
        if (availableSplits.AvailableSplits < factorSplits + durationSplits + cooldownSplits)
        {
            throw new UserOperationException($"Cannot start boost, because not enough splits available ({availableSplits.AvailableSplits})");
        }
        
        if(factorSplits % SplitHelper.FactorSplitCost != 0 || durationSplits % SplitHelper.DurationSplitCost != 0 || cooldownSplits % SplitHelper.CooldownSplitCost != 0)
        {
            throw new UserOperationException("Invalid split count provided");
        }

        var factor = SplitHelper.CalculateFactorBoost(factorSplits);
        var durationSeconds = SplitHelper.CalculateDurationSecondsBoost(durationSplits);
        var cooldownSeconds = SplitHelper.CalculateCooldownSecondsBoost(cooldownSplits);

        var boost = new DropBoostEntity
        {
            Login = member.Login,
            Factor = factor.ToString(),
            CooldownBonusS = cooldownSeconds * 1000,
            DurationS = durationSeconds * 1000,
            StartUtcs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()
        };
        
        db.DropBoosts.RemoveRange(db.DropBoosts.Where(boost => boost.Login == member.Login)); // TODO allow multiple boosts
        await db.SaveChangesAsync();

        db.DropBoosts.Add(boost);
        await db.SaveChangesAsync();
    }
}