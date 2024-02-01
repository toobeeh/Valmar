using System.Text.Json.Serialization;

namespace Valmar.Domain.Classes.JSON;

public record MemberJson(
    [property: JsonPropertyName("UserID")] string UserId,
    string UserName,
    string UserLogin,
    GuildPropertiesJson[] Guilds
    );