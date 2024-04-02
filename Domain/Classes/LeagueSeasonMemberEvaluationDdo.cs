using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Classes;

public record LeagueSeasonMemberEvaluationDdo(
    int Year, 
    int Month, 
    double Score,
    int Count,
    int MaxStreak,
    int CurrentStreak,
    double AverageWeight,
    double AverageTime,
    DateTimeOffset SeasonStart,
    DateTimeOffset SeasonEnd
    );
