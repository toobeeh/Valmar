namespace tobeh.Valmar.Server.Domain;

public record SkribblLobbyStateDdo(
    string LobbyId,
    int Round,
    int? OwnerId,
    int? DrawerId,
    SkribblLobbySkribblSettingsDdo Settings,
    List<SkribblLobbySkribblPlayerDdo> Players
);

public record SkribblLobbySkribblSettingsDdo(string Language, int Players, int DrawTime, int Rounds);

public record SkribblLobbySkribblPlayerDdo(string Name, int PlayerId, int Score, bool Guessed);