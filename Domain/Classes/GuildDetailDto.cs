namespace Valmar.Domain.Classes;

public record GuildDetailDto(
    long GuildId,
    long ChannelId,
    long MessageId,
    int ObserveToken,
    string Name,
    int ConnectedMemberCount
    );