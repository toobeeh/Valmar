using System.Text;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Util;

public static class InventoryHelper
{
    public static SceneInventoryDdo ParseSceneInventory(string scenes)
    {
        int? activeScene = null;
        int? activeShift = null;

        var ids = scenes
            .Split(",")
            .Select(shift =>
            {
                var active = shift.Contains('.');
                var split = shift.Replace(".", "").Split(':');
                var validSceneId = int.TryParse(split.Length > 0 ? split[0] : "", out var scene);
                var validShiftValue = int.TryParse(split.Length > 1 ? split[1] : "", out var shiftValue);


                if (active && validSceneId) activeScene = scene;
                if (active && validShiftValue) activeShift = shiftValue;

                return new SceneInventoryItemDdo(validSceneId ? scene : 0, validShiftValue ? shiftValue : null);
            })
            .Where(scene => scene.SceneId > 0)
            .ToList();

        return new SceneInventoryDdo(activeScene, activeShift, ids);
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
            .Where(sprite => sprite.Sprite > 0);

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
        return string.Join(",",
            slots.Select(slot => $"{new StringBuilder(slot.Slot).Insert(0, ".", slot.Slot)}{slot.SpriteId}"));
    }

    public static string SerializeSceneInventory(SceneInventoryDdo inv)
    {
        return string.Join(",",
            inv.Scenes.Select(scene =>
                $"{(scene.SceneId == inv.ActiveId && scene.SceneShift == inv.ActiveShift ? "." : "")}{scene.SceneId}{(scene.SceneShift is not null ? $":{scene.SceneShift}" : "")}"));
    }

    public static List<MemberSpriteSlotDdo> ParseActiveSlotsFromInventory(string sprites, string shifts)
    {
        return ParseSpriteInventory(sprites, shifts).Where(slot => slot.Slot > 0).ToList();
    }

    public static int GetAwardPackRarity(int bubblesCollected)
    {
        return bubblesCollected switch
        {
            < 2500 => 1,
            < 5000 => 2,
            < 15000 => 3,
            _ => 4
        };
    }

    public static Tuple<long?, DateTimeOffset> ParsePatronizedMember(string? patronized)
    {
        if (patronized == null || string.IsNullOrWhiteSpace(patronized))
        {
            return new Tuple<long?, DateTimeOffset>(null, DateTimeOffset.FromUnixTimeSeconds(0));
        }

        var split = patronized.Split('#');
        if (split.Length != 2) throw new ArgumentException($"patronize string was in invalid format: {patronized}");

        long? targetId = null;
        if (long.TryParse(split.Length > 0 ? split[0] : "", out var id))
        {
            targetId = id;
        }

        var date = DateTimeOffset.ParseExact(split[1], "MM/dd/yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);

        return new Tuple<long?, DateTimeOffset>(targetId, date);
    }

    public static string SerializePatronizedMember(Tuple<long?, DateTimeOffset> patronized)
    {
        return $"{patronized.Item1}#{patronized.Item2:MM/dd/yyyy HH:mm:ss}";
    }

    public static int GetSlotBaseCount(double drops)
    {
        return drops switch
        {
            > 30000 => 7 + Convert.ToInt32(Math.Floor((drops - 30000) / 20000)),
            > 15 => 6,
            > 8 => 5,
            > 4 => 4,
            > 2 => 3,
            > 1 => 2,
            _ => 1
        };
    }
}