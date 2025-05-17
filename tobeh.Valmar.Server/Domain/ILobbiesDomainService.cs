using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface ILobbiesDomainService
{
    Task<List<PastDropEntity>> GetLobbyDrops(string key);
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