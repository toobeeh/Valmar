namespace Valmar.Domain.Classes;

public record MemberDdo(
    int Bubbles,
    int Drops,
    string Sprites,
    string Scenes,
    int Flags,
    string RainbowSprites,
    long DiscordId,
    string Username,
    int Login,
    List<int> ServerConnections
    );