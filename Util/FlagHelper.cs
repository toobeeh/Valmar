using Valmar.Domain.Classes;

namespace Valmar.Util;

public static class FlagHelper
{
    public const ushort 
        BubbleFarming = 1, 
        BotAdmin = 2, 
        Moderator = 4, 
        UnlimitedCloud = 8, 
        Patron = 16, 
        PermanentBan = 32, 
        DropBan = 64, 
        Patronizer = 128, 
        Booster = 256, 
        Beta = 512;
    
    public static bool HasFlag(int flags, MemberFlagDdo flag) => ((MemberFlagDdo)flags).HasFlag(flag);
    
    public static List<MemberFlagDdo> GetFlags(int flags) => Enum.GetValues<MemberFlagDdo>().Where(f => HasFlag(flags, f)).ToList();
}