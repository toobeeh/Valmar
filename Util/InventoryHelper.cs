using Valmar.Domain.Classes;

namespace Valmar.Util;

public static class InventoryHelper
{
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
            .Where(sprite => sprite.Sprite != -1 && sprite.Slot > 0);

        return spritesList
            .Select(sprite =>
                {
                    var hasShift = shiftDict.TryGetValue(sprite.Sprite, out var shift);
                    return new MemberSpriteSlotDdo(sprite.Slot, sprite.Sprite, hasShift ? shift : null);
                })
            .ToList();
    }

    public static int? ParseSceneInventory(string scenes)
    {
        foreach (var scene in scenes.Split(","))
        {
            if(!scene.Contains('.')) continue;
            var validSceneId = int.TryParse(scene.Replace(".", ""), out var sceneId);
            if(!validSceneId) continue;
            return sceneId;
        }

        return null;
    }
}