namespace tobeh.Valmar.Server.Domain.Classes;

public record EventDropDdo(string Name, int Id, string Url, int EventId, DateTimeOffset ReleaseStart, DateTimeOffset ReleaseEnd);