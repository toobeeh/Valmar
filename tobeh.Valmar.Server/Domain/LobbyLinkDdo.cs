namespace tobeh.Valmar.Server.Domain;

public record LobbyLinkDdo(long GuildId, int Login, string Link, bool SlotAvailable, string Username);