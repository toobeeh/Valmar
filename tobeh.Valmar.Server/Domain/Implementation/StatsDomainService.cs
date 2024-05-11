using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Util.NChunkTree.Bubbles;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class StatsDomainService(
    ILogger<StatsDomainService> logger,
    PalantirContext db,
    BubbleChunkTreeProvider bubbleChunks) : IStatsDomainService
{
    public async Task<BubbleTimespanRangeDdo> GetMemberBubblesInRange(int login, DateTimeOffset start,
        DateTimeOffset end)
    {
        logger.LogTrace("GetMemberBubblesInRange(login={login}, start={start}, end={end})", login, start, end);

        var chunk = bubbleChunks.GetTree().Chunk;
        var range = await chunk.GetAmountCollectedInTimespan(login, start, end);

        return new BubbleTimespanRangeDdo(range.StartAmount ?? 0, range.EndAmount ?? (range.StartAmount ?? 0));
    }

    public async Task<List<BubbleProgressEntryDdo>> GetBubbleProgress(int login, DateTimeOffset start,
        DateTimeOffset end, BubbleProgressIntervalModeDdo mode)
    {
        logger.LogTrace("GetBubbleProgress(login={login}, start={start}, end={end}, mode={mode})", login, start, end,
            mode);

        var chunk = bubbleChunks.GetTree().Chunk;
        var progress = await chunk.GetBubbleProgress(login, start, end, mode);

        return progress;
    }

    private async Task<Dictionary<int, int>> CalculateAwardScores(List<MemberDdo> members)
    {
        logger.LogTrace("CalculateAwardScores(members={members})", members);

        var logins = members.Select(m => (int?)m.Login).ToList();
        var awards = await db.Awardees
            .Where(entity => logins.Contains(entity.AwardeeLogin))
            .Join(db.Awards, inner => inner.Award, outer => outer.Id, (awardee, award) => new
            {
                Login = awardee.AwardeeLogin,
                Rarity = award.Rarity
            })
            .GroupBy(result => result.Login)
            .Select(group => new { Login = group.Key, Score = group.Sum(result => result.Rarity) })
            .ToListAsync();

        return awards.ToDictionary(award => award.Login ?? throw new NullReferenceException(), award => award.Score);
    }

    public async Task<List<LeaderboardRankDdo>> CreateLeaderboard(List<MemberDdo> members, LeaderboardModeDdo mode)
    {
        logger.LogTrace("CreateLeaderboard(members={members}, mode={mode})", members, mode);

        var awardScores = await CalculateAwardScores(members);

        // map members to leaderboard stats
        var leaderboard = members.Select(member => new
        {
            Member = member,
            Bubbles = member.Bubbles,
            Drops = member.Drops,
            AwardScore = awardScores.GetValueOrDefault(member.Login, 0)
        });

        var ranking = leaderboard
            .Where(member => !member.Member.MappedFlags.Any(flag =>
                flag is MemberFlagDdo.PermaBan or MemberFlagDdo.BubbleFarming or MemberFlagDdo.DropBan))
            .OrderByDescending(member => mode switch
            {
                LeaderboardModeDdo.Bubbles => member.Bubbles,
                LeaderboardModeDdo.Awards => member.AwardScore,
                _ => member.Drops
            })
            .Select((member, index) => new LeaderboardRankDdo(index + 1, member.Member.Login, member.Member.DiscordId,
                member.Bubbles, member.AwardScore, Convert.ToInt32(member.Drops), member.Member.Username))
            .ToList();

        return ranking;
    }
}