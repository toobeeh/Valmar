namespace tobeh.Valmar.Server.Domain.Classes;

public record ClaimDropResultDdo(long DropId, bool FirstClaim, bool ClearedDrop, int CatchMs, double LeagueWeight);