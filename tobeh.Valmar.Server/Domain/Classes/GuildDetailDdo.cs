namespace tobeh.Valmar.Server.Domain.Classes;

public record GuildDetailDdo(
    long GuildId,
    long ChannelId,
    long MessageId,
    int ObserveToken,
    string Name,
    int ConnectedMemberCount
    );