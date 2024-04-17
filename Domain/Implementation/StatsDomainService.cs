using Valmar.Domain.Classes;
using Valmar.Util.NChunkTree.Bubbles;

namespace Valmar.Domain.Implementation;

public class StatsDomainService(
    ILogger<StatsDomainService> logger, 
    BubbleChunkTreeProvider bubbleChunks) : IStatsDomainService
{
    public async Task<BubbleTimespanRangeDdo> GetMemberBubblesInRange(int login, DateTimeOffset start, DateTimeOffset end)
    {
        logger.LogTrace("GetMemberBubblesInRange(login={login}, start={start}, end={end})", login, start, end);

        var chunk = bubbleChunks.GetTree().Chunk;
        var range = await chunk.GetAmountCollectedInTimespan(login, start, end);

        return new BubbleTimespanRangeDdo(range.StartAmount ?? 0, range.EndAmount ?? (range.StartAmount ?? 0));
    }
    
    public async Task<List<BubbleProgressEntryDdo>> GetBubbleProgress(int login, DateTimeOffset start, DateTimeOffset end, BubbleProgressIntervalModeDdo mode)
    {
        logger.LogTrace("GetBubbleProgress(login={login}, start={start}, end={end}, mode={mode})", login, start, end, mode);

        var chunk = bubbleChunks.GetTree().Chunk;
        var progress = await chunk.GetBubbleProgress(login, start, end, mode);

        return progress;
    }

    public Task<List<LeaderboardRankDdo>> CreateLeaderboard(List<MemberDdo> members, LeaderboardModeDdo mode)
    {
        logger.LogTrace("CreateLeaderboard(members={members}, mode={mode})", members, mode);
        
        // map members to leaderboard stats
        var leaderboard = members.Select(member => new
        {
            Member = member,
            Bubbles = member.Bubbles,
            Drops = member.Drops
        });
        
        var ranking = leaderboard.OrderByDescending(member => mode == LeaderboardModeDdo.Bubbles ? member.Bubbles : member.Drops)
            .Select((member, index) => new LeaderboardRankDdo(index + 1, member.Member.Login, member.Member.DiscordId, member.Bubbles, Convert.ToInt32(member.Drops), member.Member.Username))
            .ToList();

        return Task.FromResult(ranking);
    }
}