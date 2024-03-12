namespace Valmar.Domain.Classes;

public record OnlineMemberDdo(int Login, int Bubbles, List<PalantirLobbyJson> JoinedLobbies, string? PatronEmoji = null);