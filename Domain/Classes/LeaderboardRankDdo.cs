namespace Valmar.Domain.Classes;

public enum LeaderboardMode
{
    Bubbles,
    Drops
}

public record LeaderboardRankDdo(int Rank, int Login, long DiscordId, int Bubbles, int Drops, string Username);