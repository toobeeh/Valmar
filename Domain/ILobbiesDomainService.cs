using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;

namespace Valmar.Domain;

public interface ILobbiesDomainService
{
    Task<List<PalantirLobbyJson>> GetPalantirLobbies();
    Task<SkribblLobbyReportJson> GetSkribblLobbyDetails(string id);
    Task<List<PalantirLobbyPlayerDdo>> GetPalantirLobbyPlayers(PalantirLobbyJson palantirLobby, SkribblLobbyReportJson skribblLobby);
    Task<List<PastDropEntity>> GetLobbyDrops(string key);
}