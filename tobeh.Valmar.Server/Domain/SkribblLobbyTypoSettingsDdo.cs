namespace tobeh.Valmar.Server.Domain;

public record SkribblLobbyTypoSettingsDdo(
    string LobbyId,
    string Description,
    bool WhitelistAllowedServers,
    List<long> AllowedServers,
    long? LobbyOwnershipClaim,
    DateTimeOffset FirstSeen,
    DateTimeOffset LastUpdated
);