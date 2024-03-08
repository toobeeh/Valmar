namespace Valmar.Util;

public static class FlagHelper
{
    public static readonly ushort 
        BubbleFarming = 0, 
        BotAdmin = 1, 
        Moderator = 2, 
        UnlimitedCloud = 3, 
        Patron = 5, 
        PermanentBan = 6, 
        DropBan = 6, 
        Patronizer = 7, 
        Booster = 8, 
        Beta = 9;
    
    public static bool HasFlag(int flags, ushort flag) => (flags & (1 << flag)) != 0;
}