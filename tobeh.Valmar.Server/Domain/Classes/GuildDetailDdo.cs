namespace tobeh.Valmar.Server.Domain.Classes;

public record GuildDetailDdo(
    long GuildId,
    int Invite,
    string Name,
    int ConnectedMemberCount,
    int OnlineMemberCount,
    List<int> Supporters,
    long? BotId
);