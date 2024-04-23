using System.Text.Json.Serialization;

namespace tobeh.Valmar.Server.Domain.Classes.JSON;

public record GuildPropertiesJson(
    [property: JsonPropertyName("GuildID")] string GuildId,
    [property: JsonPropertyName("ChannelID")] string ChannelId,
    [property: JsonPropertyName("MessageID")] string MessageId,
    string ObserveToken,
    string GuildName
    );