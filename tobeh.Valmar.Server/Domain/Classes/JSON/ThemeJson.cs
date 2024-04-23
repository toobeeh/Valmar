namespace tobeh.Valmar.Server.Domain.Classes.JSON;

public record ThemeJson(ThemeMetaJson Meta);

public record ThemeMetaJson(string Author, string Name, ulong Created, string Type, ulong Id);