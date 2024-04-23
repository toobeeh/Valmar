namespace tobeh.Valmar.Server.Domain.Classes;

public record OutfitDdo(string Name, List<MemberSpriteSlotDdo> SpriteSlotConfiguration, int? SceneId);