namespace tobeh.Valmar.Server.Domain.Classes;

public record SpriteDdo(int Id, string Name, string Url, int Cost, bool Special, int EventDropId, string? Artist, int Rainbow, bool Released);