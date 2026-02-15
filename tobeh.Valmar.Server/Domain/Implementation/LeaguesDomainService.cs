using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Util;
using tobeh.Valmar.Server.Util.NChunkTree.Drops;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class LeaguesDomainService(
    ILogger<AdminDomainService> logger,
    IMembersDomainService memberService,
    DropChunkTreeProvider dropChunks) : ILeaguesDomainService
{
    public async Task<LeagueSeasonMemberEvaluationDdo> EvaluateOwnLeagueSeason(int year, int month, int login)
    {
        logger.LogTrace("EvaluateOwnLeagueSeason({year}, {month}, {login})", year, month, login);

        var member = await memberService.GetMemberByLogin(login);
        var chunk = dropChunks.GetTree().Chunk;
        var dateStart = new DateTimeOffset(new DateTime(year, month, 1, 0, 0, 1, DateTimeKind.Utc));
        var dateEnd = new DateTimeOffset(new DateTime(year, month, 1, 0, 0, 1, DateTimeKind.Utc).AddMonths(1));
        var participants = await chunk.GetLeagueResults(dateStart, dateEnd);

        if (participants.TryGetValue(member.DiscordId.ToString(), out var userResult))
        {
            return new LeagueSeasonMemberEvaluationDdo(
                year,
                month,
                userResult.Score,
                userResult.Count,
                userResult.Streak.Streak,
                userResult.Streak.Head,
                userResult.AverageWeight,
                userResult.AverageTime,
                dateStart,
                dateEnd);
        }

        return new LeagueSeasonMemberEvaluationDdo(year, month, 0, 0, 0, 0, 0, 0, dateStart, dateEnd);
    }

    public async Task<LeagueSeasonSplitEvaluationDdo> EvaluateLeagueSeasonSplits(int year, int month)
    {
        logger.LogTrace("EvaluateLeagueSeasonSplits({year}, {month})", year, month);

        var season = await EvaluateLeagueSeason(year, month);
        var rewards = new Dictionary<long, List<LeagueSeasonMemberSplitEvaluationDdo>>();

        var winner = season.ScoreRanking.FirstOrDefault();
        if (winner is not null)
        {
            /* overall #1 */
            rewards.AddToList(winner.UserId,
                new LeagueSeasonMemberSplitEvaluationDdo(winner.Name, 6, winner.UserId,
                    $"Overall Ranking Leader ({winner.Score}dw)"));

            /* overall #2 */
            var secondPlace = season.ScoreRanking.Skip(1).FirstOrDefault();
            if (secondPlace is not null)
            {
                rewards.AddToList(secondPlace.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(winner.Name, 4, winner.UserId,
                        $"Overall Ranking #2 ({secondPlace.Score:F1}dw)"));
            }

            /* overall #3 */
            var thirdPlace = season.ScoreRanking.Skip(2).FirstOrDefault();
            if (thirdPlace is not null)
            {
                rewards.AddToList(thirdPlace.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(winner.Name, 3, winner.UserId,
                        $"Overall Ranking #3 ({thirdPlace.Score:F1}dw)"));
            }

            /* top 10 */
            foreach (var place in season.ScoreRanking.Skip(3).Take(7))
            {
                rewards.AddToList(place.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(place.Name, 2, place.UserId,
                        $"Overall Ranking Top 10 ({place.Score:F1}dw)"));
            }

            /* top 20 */
            foreach (var place in season.ScoreRanking.Skip(10).Take(10))
            {
                rewards.AddToList(place.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(place.Name, 1, place.UserId,
                        $"Overall Ranking Top 20 ({place.Score:F1}dw)"));
            }

            var streakWithoutWinner = season.StreakRanking.Where(s => s.UserId != winner.UserId).ToList();

            /* streak #1 */
            var streakWinner = streakWithoutWinner.FirstOrDefault();
            if (streakWinner is not null)
            {
                rewards.AddToList(streakWinner.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(streakWinner.Name, 2, streakWinner.UserId,
                        $"Longest Streak ({streakWinner.MaxStreak})"));
            }

            /* streak #2 */
            var streakSecondPlace = streakWithoutWinner.Skip(1).FirstOrDefault();
            if (streakSecondPlace is not null)
            {
                rewards.AddToList(streakSecondPlace.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(streakSecondPlace.Name, 1, streakSecondPlace.UserId,
                        $"#2 Streak ({streakSecondPlace.MaxStreak})"));
            }

            /* streak #3 */
            var streakThirdPlace = streakWithoutWinner.Skip(2).FirstOrDefault();
            if (streakThirdPlace is not null)
            {
                rewards.AddToList(streakThirdPlace.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(streakThirdPlace.Name, 1, streakThirdPlace.UserId,
                        $"#3 Streak ({streakThirdPlace.MaxStreak})"));
            }

            var countWithoutWinner = season.CountRanking.Where(s => s.UserId != winner.UserId).ToList();

            /* count #1 */
            var countWinner = countWithoutWinner.FirstOrDefault();
            if (countWinner is not null)
            {
                rewards.AddToList(countWinner.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(countWinner.Name, 2, countWinner.UserId,
                        $"Most Drops ({countWinner.CaughtDrops})"));
            }

            /* count #2 */
            var countSecondPlace = countWithoutWinner.Skip(1).FirstOrDefault();
            if (countSecondPlace is not null)
            {
                rewards.AddToList(countSecondPlace.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(countSecondPlace.Name, 1, countSecondPlace.UserId,
                        $"#2 Drops ({countSecondPlace.CaughtDrops})"));
            }

            /* count #3 */
            var countThirdPlace = countWithoutWinner.Skip(2).FirstOrDefault();
            if (countThirdPlace is not null)
            {
                rewards.AddToList(countThirdPlace.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(countThirdPlace.Name, 1, countThirdPlace.UserId,
                        $"#3 Drops ({countThirdPlace.CaughtDrops})"));
            }

            /* league champion */
            if (season.CountRanking.FirstOrDefault()?.UserId == winner.UserId &&
                season.StreakRanking.FirstOrDefault()?.UserId == winner.UserId)
            {
                rewards.AddToList(winner.UserId,
                    new LeagueSeasonMemberSplitEvaluationDdo(winner.Name, 4, winner.UserId,
                        "League Champion in all categories"));
            }

            /* aggregate user rewards */
            var userRewards = rewards.Select(kvp => new LeagueSeasonMemberSplitEvaluationDdo(
                    season.ScoreRanking.First(r => r.UserId == kvp.Key).Name,
                    kvp.Value.Sum(r => r.Splits),
                    kvp.Key,
                    string.Join(", ", kvp.Value.Select(r => r.Comment))
                ))
                .OrderByDescending(item => item.Splits)
                .ToList();

            return new LeagueSeasonSplitEvaluationDdo(year, month, userRewards, season.SeasonStart, season.SeasonEnd);
        }
        else
        {
            return new LeagueSeasonSplitEvaluationDdo(year, month, [], season.SeasonStart, season.SeasonEnd);
        }
    }

    public async Task<LeagueSeasonEvaluationDdo> EvaluateLeagueSeason(int year, int month)
    {
        logger.LogTrace("EvaluateLeagueSeason({year}, {month})", year, month);

        var chunk = dropChunks.GetTree().Chunk;
        var dateStart = new DateTimeOffset(new DateTime(year, month, 1, 0, 0, 1, DateTimeKind.Utc));
        var dateEnd = new DateTimeOffset(new DateTime(year, month, 1, 0, 0, 1, DateTimeKind.Utc).AddMonths(1));

        var participants = await chunk.GetLeagueResults(dateStart, dateEnd);

        var participantDetails =
            await memberService.GetMemberInfosFromDiscordIds(participants.Keys.ToList()
                .Select(id => Convert.ToInt64(id)).ToList());

        var memberInfoDict = participantDetails.ToDictionary(detail => detail.UserId);

        var scoreRanking = participants
            .Select(p =>
                new LeagueScoreRankingDdo(memberInfoDict[p.Key].UserName, p.Value.Score, Convert.ToInt64(p.Key)))
            .OrderByDescending(item => item.Score)
            .ToList();

        var countRanking = participants
            .Select(p =>
                new LeagueCountRankingDdo(memberInfoDict[p.Key].UserName, p.Value.Count, Convert.ToInt64(p.Key)))
            .OrderByDescending(item => item.CaughtDrops)
            .ToList();

        var streakRanking = participants
            .Select(p =>
                new LeagueStreakRankingDdo(memberInfoDict[p.Key].UserName, p.Value.Streak.Streak, p.Value.Streak.Head,
                    Convert.ToInt64(p.Key)))
            .OrderByDescending(item => item.MaxStreak)
            .ToList();

        var weightRanking = participants
            .Select(p =>
                new LeagueAverageWeightRankingDdo(memberInfoDict[p.Key].UserName, p.Value.AverageWeight,
                    Convert.ToInt64(p.Key)))
            .OrderByDescending(item => item.AverageWeight)
            .ToList();

        var timeRanking = participants
            .Select(p =>
                new LeagueAverageTimeRankingDdo(memberInfoDict[p.Key].UserName, p.Value.AverageTime,
                    Convert.ToInt64(p.Key)))
            .OrderBy(item => item.AverageTime)
            .ToList();

        return new LeagueSeasonEvaluationDdo(year, month, scoreRanking, countRanking, weightRanking, timeRanking,
            streakRanking, dateStart, dateEnd);
    }
}