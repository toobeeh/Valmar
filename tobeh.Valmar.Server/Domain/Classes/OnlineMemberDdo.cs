namespace tobeh.Valmar.Server.Domain.Classes;

public record OnlineMemberDdo(
    int Login,
    int Bubbles,
    List<JoinedLobbyDdo> JoinedLobbies,
    string? PatronEmoji,
    List<MemberSpriteSlotDdo> SpriteSlots,
    int? SceneId);

public record JoinedLobbyDdo(int LobbyPlayerId, PalantirLobbyJson Lobby);