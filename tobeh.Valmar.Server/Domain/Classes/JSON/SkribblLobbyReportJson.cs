using System.Text.Json.Serialization;

namespace tobeh.Valmar.Server.Domain.Classes.JSON;

public record SkribblLobbyReportJson(
    string Language,
    string Link,
    bool Private,
    int Round,
    [property: JsonPropertyName("GuildID")]
    string GuildId,
    string ObserveToken,
    SkribblLobbyPlayerReportJson[] Players
);

public record SkribblLobbyPlayerReportJson(
    string Name,
    string Score,
    bool Drawing,
    bool Sender,
    [property: JsonPropertyName("LobbyPlayerID")] string LobbyPlayerId);