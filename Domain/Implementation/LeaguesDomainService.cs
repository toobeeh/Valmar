using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Exceptions;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation;

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
                userResult.AverageTime);
        }
        return new LeagueSeasonMemberEvaluationDdo(year, month, 0, 0, 0, 0, 0, 0);
    }

    public async Task<LeagueSeasonEvaluationDdo> EvaluateLeagueSeason(int year, int month) // TODO fix streak calc with drops inbetween
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
                new LeagueScoreRankingDdo(memberInfoDict[p.Key].UserName, p.Value.Score))
            .OrderByDescending(item => item.Score)
            .ToList();
        
        var countRanking = participants
            .Select(p =>
            new LeagueCountRankingDdo(memberInfoDict[p.Key].UserName, p.Value.Count))
            .OrderByDescending(item => item.CaughtDrops)
            .ToList();
        
        var streakRanking = participants
            .Select(p =>
            new LeagueStreakRankingDdo(memberInfoDict[p.Key].UserName, p.Value.Streak.Streak, p.Value.Streak.Head))
            .OrderByDescending(item => item.MaxStreak)
            .ToList();
        
        var weightRanking = participants
            .Select(p =>
            new LeagueAverageWeightRankingDdo(memberInfoDict[p.Key].UserName, p.Value.AverageWeight))
            .OrderByDescending(item => item.AverageWeight)
            .ToList();
        
        var timeRanking = participants
            .Select(p =>
            new LeagueAverageTimeRankingDdo(memberInfoDict[p.Key].UserName, p.Value.AverageTime))
            .OrderBy(item => item.AverageTime)
            .ToList();
        
        return new LeagueSeasonEvaluationDdo(year, month, scoreRanking, countRanking, weightRanking, timeRanking, streakRanking);
    }
}