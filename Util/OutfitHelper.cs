using System.Text;
using Valmar.Domain.Classes;

namespace Valmar.Util;

public static class OutfitHelper
{
    public static string SerializeSimpleCombo(List<MemberSpriteSlotDdo> slots)
    {
        return string.Join(",", slots.Select(slot => slot.SpriteId));
    }
    
    public static string SerializeSimpleColorConfig(List<MemberSpriteSlotDdo> slots)
    {
        return string.Join(",", slots
            .Where(slot => slot.ColorShift is not null)
            .Select(slot => $"{slot.SpriteId}:{slot.ColorShift}"));
    }

    public static int? ParseSceneFromOutfit(string outfitScene)
    {
        if (string.IsNullOrWhiteSpace(outfitScene)) return null;
        
        var scene = outfitScene.Split(",");
        if (string.IsNullOrWhiteSpace(scene[0])) return null;
        
        return int.TryParse(scene[0], out var sceneId) ? sceneId : null;
    }

    public static List<MemberSpriteSlotDdo> ParseComboFromOutfit(string combo, string rainbows)
    {
        return InventoryHelper.ParseActiveSlotsFromInventory(
            string.Join(",", combo.Split(",").Select((item, idx) => $"{".".Repeat(idx + 1)}{item}")),
            rainbows);
    }
}