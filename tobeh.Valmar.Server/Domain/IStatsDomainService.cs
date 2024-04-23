using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Util.NChunkTree.Bubbles;

namespace tobeh.Valmar.Server.Domain;

public interface IStatsDomainService
{
    Task<BubbleTimespanRangeDdo> GetMemberBubblesInRange(int login, DateTimeOffset start, DateTimeOffset end);
    Task<List<LeaderboardRankDdo>> CreateLeaderboard(List<MemberDdo> members, LeaderboardModeDdo mode);
    Task<List<BubbleProgressEntryDdo>> GetBubbleProgress(int login, DateTimeOffset start, DateTimeOffset end, BubbleProgressIntervalModeDdo mode);
}