namespace tobeh.Valmar.Server.Domain.Classes;

public record SkribblLobbyTypoMemberDdo(
    int Login,
    int LobbyPlayerId,
    long OwnershipClaim,
    int Bubbles,
    string? PatronEmoji,
    int? SceneId,
    int? SceneShift,
    List<MemberSpriteSlotDdo> SpriteSlots);