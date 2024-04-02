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

public record LeagueScoreRankingDdo(string Name, double Score);
public record LeagueCountRankingDdo(string Name, int CaughtDrops);
public record LeagueAverageWeightRankingDdo(string Name, double AverageWeight);
public record LeagueAverageTimeRankingDdo(string Name, double AverageTime);
public record LeagueStreakRankingDdo(string Name, int MaxStreak, int CurrentStreak);