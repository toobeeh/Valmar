using tobeh.Valmar.Server.Util;

namespace tobeh.Valmar.Server.Domain.Classes;

[Flags]
public enum MemberFlagDdo : ushort
{
    Admin = FlagHelper.BotAdmin,
    Moderator = FlagHelper.Moderator,
    Patron = FlagHelper.Patron,
    Patronizer = FlagHelper.Patronizer,
    Booster = FlagHelper.Booster,
    DropBan = FlagHelper.DropBan,
    PermaBan = FlagHelper.PermanentBan,
    Beta = FlagHelper.Beta,
    BubbleFarming = FlagHelper.BubbleFarming,
    UnlimitedCloud = FlagHelper.UnlimitedCloud,
    ContentModerator = FlagHelper.ContentModerator,
    EmojiManagement = FlagHelper.EmojiManagement
}