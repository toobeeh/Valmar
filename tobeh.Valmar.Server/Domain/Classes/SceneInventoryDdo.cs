namespace tobeh.Valmar.Server.Domain.Classes;

public record SceneInventoryItemDdo(int SceneId, int? SceneShift);

public record SceneInventoryDdo(int? ActiveId, int? ActiveShift, List<SceneInventoryItemDdo> Scenes);