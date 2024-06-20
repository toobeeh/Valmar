using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Util;

public static class OutfitHelper
{
    public static string SerializeSimpleCombo(List<MemberSpriteSlotDdo> slots)
    {
        return string.Join(",", slots.OrderBy(slot => slot.Slot).Select(slot => slot.SpriteId));
    }

    public static string SerializeSimpleColorConfig(List<MemberSpriteSlotDdo> slots)
    {
        return string.Join(",", slots
            .Where(slot => slot.ColorShift is not null)
            .Select(slot => $"{slot.SpriteId}:{slot.ColorShift}"));
    }

    public static SceneInventoryItemDdo? ParseSceneFromOutfit(string outfitScene)
    {
        if (string.IsNullOrWhiteSpace(outfitScene)) return null;

        var shift = outfitScene.Split(",");
        if (string.IsNullOrWhiteSpace(shift[0]))
            return null; // TODO fix inceonsistency in db where multiple scenes are saved wth

        var split = shift[0].Split(':');
        var validSceneId = int.TryParse(split.Length > 0 ? split[0] : "", out var scene);
        var validShiftValue = int.TryParse(split.Length > 1 ? split[1] : "", out var shiftValue);
        return new SceneInventoryItemDdo(validSceneId ? scene : 0, validShiftValue ? shiftValue : null);
    }

    public static List<MemberSpriteSlotDdo> ParseComboFromOutfit(string combo, string rainbows)
    {
        return InventoryHelper.ParseActiveSlotsFromInventory(
            string.Join(",", combo.Split(",").Select((item, idx) => $"{".".Repeat(idx + 1)}{item}")),
            rainbows);
    }

    public static string SerializeScene(SceneInventoryItemDdo? scene)
    {
        if (scene is null) return "";
        return scene.SceneId + (scene.SceneShift is not null ? $":{scene.SceneShift}" : "");
    }
}