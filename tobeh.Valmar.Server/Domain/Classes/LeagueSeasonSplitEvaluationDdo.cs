namespace tobeh.Valmar.Server.Domain.Classes;

public record LeagueSeasonSplitEvaluationDdo(
    int Year,
    int Month,
    List<LeagueSeasonMemberSplitEvaluationDdo> Evaluation,
    DateTimeOffset SeasonStart,
    DateTimeOffset SeasonEnd
);

public record LeagueSeasonMemberSplitEvaluationDdo(string Name, long Splits, long UserId, string Comment);