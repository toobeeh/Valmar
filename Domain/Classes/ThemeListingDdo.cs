namespace Valmar.Domain.Classes;

public record ThemeListingDdo(
    string Name,
    int Version,
    int Downloads,
    string Id,
    string Author);