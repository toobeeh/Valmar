using Valmar.Domain.Classes;
using Valmar.Util.NChunkTree.Bubbles;

namespace Valmar.Domain;

public interface IStatsDomainService
{
    Task<BubbleTimespanRangeDdo> GetMemberBubblesInRange(int login, DateTimeOffset start, DateTimeOffset end);
    Task<List<LeaderboardRankDdo>> CreateLeaderboard(List<MemberDdo> members, LeaderboardModeDdo mode);
    Task<List<BubbleProgressEntryDdo>> GetBubbleProgress(int login, DateTimeOffset start, DateTimeOffset end, BubbleProgressIntervalModeDdo mode);
}