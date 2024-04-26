namespace tobeh.Valmar.Server.Domain.Classes;

public record LeagueEventDropValueDdo(int EventDropId, double Value);
public record DropCreditDdo(int TotalCredit, int RegularCount, int LeagueCount, List<LeagueEventDropValueDdo> LeagueEventDropValues);