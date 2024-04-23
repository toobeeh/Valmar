using System.Text;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Util;

public static class InventoryHelper
{
    public static SceneInventoryDdo ParseSceneInventory(string scenes)
    {
        int? activeScene = null;
        
        var ids = scenes
            .Split(",")
            .Select(scene =>
            {
                var active = scene.Contains('.');
                var validSceneId = int.TryParse(scene.Replace(".", ""), out var sceneId);
                
                if(active && validSceneId) activeScene = sceneId;
                
                return validSceneId ? sceneId : 0;
            })
            .Where(id => id > 0)
            .ToList();

        return new SceneInventoryDdo(activeScene, ids);
    }
    
    public static List<MemberSpriteSlotDdo> ParseSpriteInventory(string sprites, string shifts)
    {
        var shiftDict = shifts
            .Split(',')
            .Select(shift =>
            {
                var split = shift.Split(':');
                var validSpriteId = int.TryParse(split.Length > 0 ? split[0] : "", out var sprite);
                var validShiftValue = int.TryParse(split.Length > 1 ? split[1] : "", out var shiftValue);
                
                return new
                {
                    Sprite = validSpriteId ? sprite : -1,
                    Shift = validShiftValue ? shiftValue : -1
                };
            })
            .Where(shift => shift.Sprite != -1 && shift.Shift != -1)
            .ToDictionary(shift => shift.Sprite, shift => shift.Shift);

        var spritesList = sprites
            .Split(',')
            .Select(sprite =>
            {
                var slot = sprite.Count(c => c == '.');
                var validSpriteId = int.TryParse(sprite.Replace(".", ""), out var spriteId);

                return new { Slot = slot, Sprite = validSpriteId ? spriteId : -1 };
            })
            .Where(sprite => sprite.Sprite != -1);

        return spritesList
            .Select(sprite =>
                {
                    var hasShift = shiftDict.TryGetValue(sprite.Sprite, out var shift);
                    return new MemberSpriteSlotDdo(sprite.Slot, sprite.Sprite, hasShift ? shift : null);
                })
            .ToList();
    }
    
    public static string SerializeSpriteInventory(List<MemberSpriteSlotDdo> slots)
    {
        return string.Join(",", slots.Select(slot => $"{new StringBuilder(slot.Slot).Insert(0, ".", slot.Slot)}{slot.SpriteId}"));
    }
    
    public static string SerializeSceneInventory(SceneInventoryDdo inv)
    {
        return string.Join(",", inv.SceneIds.Select(id => $"{(id == inv.ActiveId ? "." : "")}{id}"));
    }

    public static List<MemberSpriteSlotDdo> ParseActiveSlotsFromInventory(string sprites, string shifts)
    {
        return ParseSpriteInventory(sprites, shifts).Where(slot => slot.Slot > 0).ToList();
    }

    public static int GetAwardPackRarity(int bubblesCollected)
    {
        return bubblesCollected switch {
            < 2500 => 1,
            < 5000 => 2,
            < 15000 => 3,
            _ => 4
        };
    }
}