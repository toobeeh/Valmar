using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Classes.JSON;

namespace tobeh.Valmar.Server.Domain;

public interface IMembersDomainService
{
    Task<MemberDdo> CreateMember(long discordId, string username, bool connectTypo);
    Task<MemberDdo> GetMemberByLogin(int login);
    Task<MemberDdo> GetMemberByDiscordId(long id);
    Task<MemberDdo> GetMemberByAccessToken(string token);
    Task<List<MemberSearchDdo>> SearchMember(string query);
    Task<string> GetRawMemberByLogin(int login);
    Task<string> GetAccessTokenByLogin(int login);
    Task UpdateMemberDiscordId(int login, long newId);
    Task ClearMemberDropboost(int login);
    Task ConnectToServer(int login, int serverToken);
    Task DisconnectFromServer(int login, int serverToken);
    Task<int> GetMemberLoginFromDiscordId(long id);
    Task<List<MemberJson>> GetMemberInfosFromDiscordIds(List<long> ids, List<MemberFlagDdo> forbiddenFlags);
    Task<MemberDdo> GetPatronizedMemberOfPatronizer(long patronizerId);
    Task<List<MemberDdo>> GetAllMembers();
    Task<List<MemberDdo>> GetGuildMembers(long guildId);
    Task<List<MemberDdo>> GetMembersByLogin(List<int> logins);
}