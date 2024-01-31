using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;
using Valmar.Domain.Exceptions;

namespace Valmar.Domain.Implementation;

public class LobbiesDomainService(
    ILogger<LobbiesDomainService> logger, 
    PalantirContext db) : ILobbiesDomainService
{
    public async Task<List<PalantirLobbyJson>> GetPalantirLobbies()
    {
        logger.LogTrace("GetPalantirLobbies()");

        // get lobbies from db - take all lobby IDs where a report is currently set
        var lobbies = await db.Lobbies.Join(
                db.Reports,
                lobby => lobby.LobbyId,
                report => report.LobbyId,
                (lobby, report) => lobby)
            .Distinct()
            .ToListAsync();
        
        // parse from json
        return lobbies.ConvertAll(lobby => 
            ValmarJsonParser.TryParse<PalantirLobbyJson>(lobby.Lobby1, logger));
    }

    public async Task<SkribblLobbyReportJson> GetSkribblLobbyDetails(string id)
    {
        logger.LogTrace("GetSkribblLobbyDetails(key={key})", id);
        
        // get latest report matching this lobby
        var report = await db.Reports
            .OrderBy(report => report.Date)
            .FirstOrDefaultAsync(report => report.LobbyId == id);

        if (report is null)
        {
            throw new EntityNotFoundException($"No report for lobby with id {id} found.");
        }
        
        // parse report JSON
        var lobbyReport = ValmarJsonParser.TryParse<SkribblLobbyReportJson>(report.Report1, logger);

        return lobbyReport;
    }

    public async Task<List<PalantirLobbyPlayerDdo>> GetPalantirLobbyPlayers(PalantirLobbyJson palantirLobby, SkribblLobbyReportJson skribblLobby)
    {
        logger.LogTrace("GetPalantirLobbyPlayers(palantirLobby={palantirLobby}, skribblLobby={skribblLobby})", palantirLobby, skribblLobby);
        
        var candidates = await db.Statuses
            .Where(status => status.Status1.Contains(palantirLobby.Id)) // filter out possible candidates to minimize parsing time
            .Select(status => status.Status1)
            .ToListAsync();

        var players = candidates
            .ConvertAll(candidate => ValmarJsonParser.TryParse<MemberStatusJson>(candidate, logger))
            .Where(status => status.LobbyId == palantirLobby.Id && status.Status == "playing")
            .ToList();

        var lobbyPlayers = skribblLobby.Players.ToList();
        return players.ConvertAll(player =>
        {
            var playerMatch =
                lobbyPlayers.FirstOrDefault(lobbyPlayer => lobbyPlayer.LobbyPlayerId == player.LobbyPlayerId);
            
            return new PalantirLobbyPlayerDdo(
                playerMatch?.Name ?? "",
                Convert.ToInt32(player.PlayerMember.UserLogin),
                playerMatch != null ? Convert.ToInt32(playerMatch.LobbyPlayerId) : -1,
                player.PlayerMember.UserName);
        });
    }

    public async Task<List<PastDropEntity>> GetLobbyDrops(string key)
    {
        logger.LogTrace("GetLobbyDrops(key={key})", key);

        var drops = await db.PastDrops.Where(drop => drop.CaughtLobbyKey == key).ToListAsync();
        return drops;
    }
}