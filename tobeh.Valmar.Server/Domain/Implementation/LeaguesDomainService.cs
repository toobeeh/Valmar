using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Util.NChunkTree.Drops;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Exceptions;

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
            new LeagueStreakRankingDdo(memberInfoDict[p.Key].UserName, p.Value.Streak.Streak, p.Value.Streak.Head, Convert.ToInt64(p.Key)))
            .OrderByDescending(item => item.MaxStreak)
            .ToList();
        
        var weightRanking = participants
            .Select(p =>
            new LeagueAverageWeightRankingDdo(memberInfoDict[p.Key].UserName, p.Value.AverageWeight, Convert.ToInt64(p.Key)))
            .OrderByDescending(item => item.AverageWeight)
            .ToList();
        
        var timeRanking = participants
            .Select(p =>
            new LeagueAverageTimeRankingDdo(memberInfoDict[p.Key].UserName, p.Value.AverageTime, Convert.ToInt64(p.Key)))
            .OrderBy(item => item.AverageTime)
            .ToList();
        
        return new LeagueSeasonEvaluationDdo(year, month, scoreRanking, countRanking, weightRanking, timeRanking, streakRanking, dateStart, dateEnd);
    }
}