using tobeh.Valmar.Server.Domain.Classes.Param;

namespace tobeh.Valmar.Server.Domain.Classes;

public record SkribblLobbyTypoMembersDdo(string LobbyId, List<SkribblLobbyTypoMemberDdo> Members);