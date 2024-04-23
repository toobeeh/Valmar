namespace tobeh.Valmar.Server.Domain;

public record GalleryItemDdo(long ImageId, string ImageUrl, string Title, string Author, DateTimeOffset Date, string Language, bool IsOwn, bool InPrivateLobby);