using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Classes;

public record LeagueSeasonEvaluationDdo(
    int Year, 
    int Month, 
    List<LeagueScoreRankingDdo> ScoreRanking, 
    List<LeagueCountRankingDdo> CountRanking, 
    List<LeagueAverageWeightRankingDdo> WeightRanking, 
    List<LeagueAverageTimeRankingDdo> TimeRanking, 
    List<LeagueStreakRankingDdo> StreakRanking,
    DateTimeOffset SeasonStart,
    DateTimeOffset SeasonEnd
    );

public record LeagueScoreRankingDdo(string Name, double Score, long UserId);
public record LeagueCountRankingDdo(string Name, int CaughtDrops, long UserId);
public record LeagueAverageWeightRankingDdo(string Name, double AverageWeight, long UserId);
public record LeagueAverageTimeRankingDdo(string Name, double AverageTime, long UserId);
public record LeagueStreakRankingDdo(string Name, int MaxStreak, int CurrentStreak, long UserId);