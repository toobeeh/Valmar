using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;
using Valmar.Domain.Exceptions;
using Valmar.Util;

namespace Valmar.Domain.Implementation;

public class LobbiesDomainService(
    ILogger<LobbiesDomainService> logger, 
    IMembersDomainService membersService,
    PalantirContext db) : ILobbiesDomainService
{
    public async Task<List<OnlineMemberDdo>> GetOnlineMembers()
    {
        logger.LogTrace("GetOnlineMembers()");

        // get online players from status table
        var statuses = await db.Statuses.Select(status => status.Status1).ToListAsync();
        var onlineMembers = statuses
            .ConvertAll(status => ValmarJsonParser.TryParse<MemberStatusJson>(status, logger))
            .Where(status => status.Status == "playing")
            .GroupBy(status => status.PlayerMember.UserLogin)
            .Select(login => new
            {
                Member = membersService.GetMemberByLogin(Convert.ToInt32(login.Key)).Result, 
                Lobbies = login.Select(status => new { Id = status.LobbyId, PlayerId = status.LobbyPlayerId}).ToList()
            })
            .ToList();

        var lobbyIds = onlineMembers.Select(member => member.Lobbies).SelectMany(m => m).Select(l => l.Id);
        var lobbiesDict = (await db.Lobbies
            .Where(lobby => lobbyIds.Contains(lobby.LobbyId))
            .ToListAsync())
            .ToDictionary(lobby => lobby.LobbyId, lobby => ValmarJsonParser.TryParse<PalantirLobbyJson>(lobby.Lobby1, logger));

        return onlineMembers
            .Select(member => new OnlineMemberDdo(
                member.Member.Login, 
                member.Member.Bubbles, 
                member.Lobbies.Select(lobby => new JoinedLobbyDdo(Convert.ToInt32(lobby.PlayerId), lobbiesDict[lobby.Id])).ToList(), 
                member.Member.PatronEmoji,
                InventoryHelper.ParseActiveSlotsFromInventory(member.Member.Sprites, member.Member.RainbowSprites),
                InventoryHelper.ParseSceneInventory(member.Member.Scenes).ActiveId
                )).ToList();
    }
    
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