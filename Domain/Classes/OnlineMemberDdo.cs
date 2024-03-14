namespace Valmar.Domain.Classes;

public record OnlineMemberDdo(
    int Login,
    int Bubbles,
    List<JoinedLobbyDdo> JoinedLobbies,
    string? PatronEmoji,
    List<MemberSpriteSlotDdo> SpriteSlots,
    int? SceneId);
public record MemberSpriteSlotDdo(int Slot, int SpriteId, int? RainbowShift = null);
public record JoinedLobbyDdo(int LobbyPlayerId, PalantirLobbyJson Lobby);