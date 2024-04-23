namespace tobeh.Valmar.Server.Domain.Classes;

public enum LeaderboardModeDdo
{
    Bubbles,
    Drops
}

public record LeaderboardRankDdo(int Rank, int Login, long DiscordId, int Bubbles, int Drops, string Username);