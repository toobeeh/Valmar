namespace tobeh.Valmar.Server.Domain.Classes;

public enum LeaderboardModeDdo
{
    Bubbles,
    Drops,
    Awards
}

public record LeaderboardRankDdo(
    int Rank,
    int Login,
    long DiscordId,
    int Bubbles,
    int AwardScore,
    int Drops,
    string Username);