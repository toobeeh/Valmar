using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Classes.JSON;
using tobeh.Valmar.Server.Domain.Classes.Param;

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
    Task<SkribblLobbyTypoSettingsDdo> GetSkribblLobbyTypoSettings(string lobbyId);

    Task<SkribblLobbyTypoSettingsDdo> SetSkribblLobbyTypoSettings(string lobbyId, long? ownershipClaim,
        string description, bool whitelistAllowedServers, IEnumerable<long> whiltelistedServers);

    Task SetMemberStatusesInSkribblLobby(string lobbyId,
        List<SkribblLobbyTypoMemberDdo> members);

    Task SetSkribblLobbyState(string lobbyId, SkribblLobbyStateDdo state);
    Task RemoveMemberStatusesInSkribblLobby(string lobbyId, List<SkribblLobbyTypoMemberDdo> members);
    Task<List<SkribblLobbyTypoMembersDdo>> GetOnlineLobbyPlayers(long? guildId);
    Task<SkribblLobbyDdo> GetLobbyById(string lobbyId);
    Task<List<SkribblLobbyDdo>> GetLobbiesById(List<string> ids);
    Task ClearOrphanedLobbyData();
}