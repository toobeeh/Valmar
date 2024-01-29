namespace Valmar.Domain.Classes.JSON;

public record GuildProperties(
    string GuildID,
    string ChannelID,
    string MessageID,
    string ObserveToken,
    string GuildName
    );