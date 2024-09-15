namespace tobeh.Valmar.Server.Domain.Classes;

public record CloudImageDdo(long Id, string ImageUrl, string MetaUrl, string CommandsUrl, CloudImageTagDdo Tags);

public record CloudImageTagDdo(
    string Title,
    string Author,
    string Language,
    DateTimeOffset CreatedAt,
    bool CreatedInPrivateLobby,
    bool IsOwn,
    int OwnerLogin);