using System.Text.Json.Serialization;

namespace tobeh.Valmar.Server.Domain.Classes;

public record PalantirLobbyJson(
    string Description,
    string Key,
    [property: JsonPropertyName("ID")] string Id,
    string Restriction);