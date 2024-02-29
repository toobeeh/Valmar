using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Classes;

public record LeagueSeasonEvaluationDdo(
    int Year, 
    int Month, 
    List<LeagueSeasonScoreRankingDdo> ScoreRanking, 
    List<LeagueSeasonCountRankingDdo> CountRanking, 
    List<LeagueSeasonAverageWeightRankingDdo> WeightRanking, 
    List<LeagueSeasonAverageTimeRankingDdo> TimeRanking, 
    List<LeagueSeasonStreakRankingDdo> StreakRanking
    );

public record LeagueSeasonScoreRankingDdo(string Name, double Score);
public record LeagueSeasonCountRankingDdo(string Name, int Count);
public record LeagueSeasonAverageWeightRankingDdo(string Name, double AverageWeight);
public record LeagueSeasonAverageTimeRankingDdo(string Name, double AverageTime);
public record LeagueSeasonStreakRankingDdo(string Name, int MaxStreak, int CurrentStreak);