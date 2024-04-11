using Valmar.Domain.Classes;
using Valmar.Util.NChunkTree.Bubbles;

namespace Valmar.Domain;

public interface ISplitsDomainService
{
    Task<List<SplitDefinitionDdo>> GetSplits();
    Task<SplitDefinitionDdo> GetSplitById(int id);
    Task<List<SplitRewardDdo>> GetMemberSplitRewards(MemberDdo member);
    Task RewardSplit(int rewardeeLogin, int splitId, string? comment, int? valueOverride);
    Task<List<DropboostDdo>> GetDropboosts(int? login = null, bool onlyActive = true);
    Task<AvailableSplitsDdo> GetAvailableSplits(MemberDdo member);
    Task StartDropboost(MemberDdo member, int factorSplits, int durationSplits, int cooldownSplits);
    Task UpgradeDropboost(MemberDdo member, DateTimeOffset startDate,  int factorSplitsIncrease, int durationSplitsIncrease, int cooldownSplitsIncrease);
}