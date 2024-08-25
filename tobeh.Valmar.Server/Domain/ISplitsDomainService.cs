using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface ISplitsDomainService
{
    Task<List<SplitDefinitionDdo>> GetSplits();
    Task<SplitDefinitionDdo> GetSplitById(int id);
    Task<List<SplitRewardDdo>> GetMemberSplitRewards(MemberDdo member);
    Task RewardSplit(int rewardeeLogin, int splitId, string? comment, int? valueOverride);
    Task<List<DropboostDdo>> GetDropboosts(int? login = null, bool onlyActive = true);
    Task<AvailableSplitsDdo> GetAvailableSplits(MemberDdo member);
    Task StartDropboost(MemberDdo member, int factorSplits, int durationSplits, int cooldownSplits);

    Task UpgradeDropboost(MemberDdo member, DateTimeOffset startDate, int factorSplitsIncrease,
        int durationSplitsIncrease, int cooldownSplitsIncrease);

    Task<SplitDefinitionDdo> CreateSplitReward(string name, string description, int value);
}