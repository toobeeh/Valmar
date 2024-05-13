using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Classes.JSON;

namespace tobeh.Valmar.Server.Domain;

public interface ILobbiesDomainService
{
    Task<List<PalantirLobbyJson>> GetPalantirLobbies();
    Task<SkribblLobbyReportJson> GetSkribblLobbyDetails(string id);

    Task<List<PalantirLobbyPlayerDdo>> GetPalantirLobbyPlayers(PalantirLobbyJson palantirLobby,
        SkribblLobbyReportJson skribblLobby);

    Task<List<PastDropEntity>> GetLobbyDrops(string key);
    Task<List<OnlineMemberDdo>> GetOnlineMembers();
    Task SetGuildLobbyLinks(long guildId, List<LobbyLinkDdo> links);
    Task<List<LobbyLinkDdo>> GetGuildLobbyLinks(long? guildId = null);
}