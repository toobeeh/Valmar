using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;
using tobeh.Valmar.Server.Util;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class LobbiesDomainService(
    ILogger<LobbiesDomainService> logger,
    IMembersDomainService membersService,
    PalantirContext db) : ILobbiesDomainService
{
    public async Task<List<PastDropEntity>> GetLobbyDrops(string key)
    {
        logger.LogTrace("GetLobbyDrops(key={key})", key);

        var drops = await db.PastDrops.Where(drop => drop.CaughtLobbyKey == key).ToListAsync();
        return drops;
    }

    public async Task<SkribblLobbyTypoSettingsDdo> GetSkribblLobbyTypoSettings(string lobbyId)
    {
        logger.LogTrace("GetSkribblLobby(lobbyId={lobbyId})", lobbyId);

        var lobby = await db.SkribblLobbies.FirstOrDefaultAsync(lobby => lobby.LobbyId == lobbyId);

        // if lobby not found or not refreshed for more than 2 days, create new
        if (lobby is null || lobby.FirstSeen < DateTimeOffset.UtcNow.AddDays(-2).ToUnixTimeMilliseconds())
        {
            // remove old lobby
            if (lobby is not null)
            {
                db.SkribblLobbies.Remove(lobby);
                await db.SaveChangesAsync();
            }

            // save new lobby
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            lobby = new SkribblLobbyEntity
            {
                LobbyId = lobbyId,
                Description = "",
                AllowedServers = "",
                WhitelistAllowedServers = false,
                LobbyOwnershipClaim = null,
                FirstSeen = now,
                LastUpdated = now,
                SkribblDetails = ""
            };

            db.SkribblLobbies.Add(lobby);
            await db.SaveChangesAsync();
        }

        return MapSkribblLobbyTypoSettingsToDdo(lobby);
    }

    public async Task<SkribblLobbyTypoSettingsDdo> SetSkribblLobbyTypoSettings(string lobbyId, long? ownershipClaim,
        string description, bool whitelistAllowedServers, IEnumerable<long> whiltelistedServers)
    {
        logger.LogTrace(
            "SetSkribblLobbyTypoSettings(lobbyId={lobbyId}, ownership={ownershipClaim}, description={description}, whitelist={whitelistAllowedServers}, whitelistServers={whiltelistedServers})",
            lobbyId, ownershipClaim, description, whitelistAllowedServers, whiltelistedServers);

        var lobby = await db.SkribblLobbies.FirstOrDefaultAsync(lobby => lobby.LobbyId == lobbyId);
        if (lobby is null)
        {
            throw new EntityNotFoundException($"No lobby with id {lobbyId} found.");
        }

        lobby.LobbyOwnershipClaim = ownershipClaim;
        lobby.Description = description;
        lobby.WhitelistAllowedServers = whitelistAllowedServers;
        lobby.AllowedServers = string.Join(",", whiltelistedServers);
        lobby.LastUpdated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        db.Update(lobby);
        await db.SaveChangesAsync();
        return MapSkribblLobbyTypoSettingsToDdo(lobby);
    }

    public async Task SetMemberStatusesInSkribblLobby(string lobbyId,
        List<SkribblLobbyTypoMemberDdo> members)
    {
        logger.LogTrace("SetMemberStatusesInSkribblLobby(lobbyId={lobbyId}, members={members})",
            lobbyId, members);

        // find members that were in the lobby in the last cycle
        var oldEntries = await db.SkribblOnlinePlayers
            .Where(status => status.LobbyId == lobbyId)
            .ToListAsync();

        // update and add member statuses
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        foreach (var member in members)
        {
            var entry = oldEntries.FirstOrDefault(old => old.Login == member.Login);
            if (entry is not null)
            {
                entry.Timestamp = now;
                db.Update(entry);
            }
            else
            {
                db.SkribblOnlinePlayers.Add(new SkribblOnlinePlayerEntity
                {
                    Login = member.Login,
                    LobbyPlayerId = member.LobbyPlayerId,
                    LobbyId = lobbyId,
                    OwnershipClaim = member.OwnershipClaim,
                    Timestamp = now
                });
            }
        }

        await db.SaveChangesAsync();
    }

    public async Task RemoveMemberStatusesInSkribblLobby(string lobbyId, List<SkribblLobbyTypoMemberDdo> members)
    {
        logger.LogTrace("RemoveMemberStatusesInSkribblLobby(lobbyId={lobbyId}, members={members})", lobbyId, members);

        // find members that were in the lobby in the last cycle
        var oldEntries = await db.SkribblOnlinePlayers
            .Where(status => status.LobbyId == lobbyId)
            .ToListAsync();

        // get removed targets
        var removedEntries = oldEntries
            .Where(old => members.Any(member => member.Login == old.Login && member.LobbyPlayerId == old.LobbyPlayerId))
            .ToList();

        db.SkribblOnlinePlayers.RemoveRange(removedEntries);
        await db.SaveChangesAsync();
    }

    public async Task SetSkribblLobbyState(string lobbyId, SkribblLobbyStateDdo state)
    {
        logger.LogTrace("SetSkribblLobbyState(lobbyId={lobbyId}, state={state})", lobbyId, state);

        var lobby = await db.SkribblLobbies.FirstOrDefaultAsync(lobby => lobby.LobbyId == lobbyId);
        if (lobby is null)
        {
            throw new EntityNotFoundException($"No lobby with id {lobbyId} found.");
        }

        lobby.SkribblDetails = JsonSerializer.Serialize(state);
        lobby.LastUpdated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        db.Update(lobby);
        await db.SaveChangesAsync();
    }

    public async Task<List<SkribblLobbyTypoMembersDdo>> GetOnlineLobbyPlayers(long? guildId)
    {
        logger.LogTrace("GetOnlineLobbyPlayers(guildId={guildId})", guildId);

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var players = guildId is not null
            ? (await db.SkribblOnlinePlayers
                .Where(status => now - status.Timestamp < 10000)
                .Join(db.ServerConnections.Where(connection => connection.GuildId == guildId),
                    status => status.Login,
                    connection => connection.Login,
                    (status, connection) => status)
                .ToListAsync())
            .ToList()
            : (await db.SkribblOnlinePlayers
                .Where(status => now - status.Timestamp < 10000)
                .ToListAsync())
            .ToList();

        var members = (await membersService.GetMembersByLogin(players.Select(player => player.Login).ToList()))
            .ToDictionary(member => member.Login);

        var lobbies = players
            .GroupBy(player => player.LobbyId)
            .Select(lobby => new SkribblLobbyTypoMembersDdo(
                lobby.Key,
                lobby.Select(member => MapSkribblLobbyTypoMemberToDdo(member, members[member.Login])).ToList()));

        return lobbies.ToList();
    }

    public async Task<SkribblLobbyDdo> GetLobbyById(string lobbyId)
    {
        logger.LogTrace("GetLobbyById(lobbyId={lobbyId})", lobbyId);

        var lobby = await db.SkribblLobbies.FirstOrDefaultAsync(lobby => lobby.LobbyId == lobbyId);
        if (lobby is null)
        {
            throw new EntityNotFoundException($"No lobby with id {lobbyId} found.");
        }

        return MapSkribblLobbyToDdo(lobby);
    }

    public async Task<List<SkribblLobbyDdo>> GetLobbiesById(List<string> ids)
    {
        logger.LogTrace("GetLobbiesById(ids={ids})", ids);

        var lobbies = await db.SkribblLobbies
            .Where(lobby => ids.Contains(lobby.LobbyId))
            .ToListAsync();

        return lobbies.Select(MapSkribblLobbyToDdo).ToList();
    }

    public async Task ClearOrphanedLobbyData()
    {
        logger.LogTrace("ClearOrphanedLobbyData()");

        // delete outdated lobbies (not refreshed for 2 days)  and status (older than 10s)
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var oldStatus = await db.SkribblOnlinePlayers
            .Where(status => status.Timestamp < now - 10000)
            .ToListAsync();
        var oldLobbies = await db.SkribblLobbies
            .Where(lobby => lobby.LastUpdated < now - (2 * 24 * 60 * 60 * 1000))
            .ToListAsync();

        db.SkribblOnlinePlayers.RemoveRange(oldStatus);
        db.SkribblLobbies.RemoveRange(oldLobbies);
        await db.SaveChangesAsync();

        // delete orphaned status
        var orphanedStatus = await db.SkribblOnlinePlayers
            .Where(status => !db.SkribblLobbies.Select(lobby => lobby.LobbyId).Contains(status.LobbyId))
            .ToListAsync();

        db.SkribblOnlinePlayers.RemoveRange(orphanedStatus);
        await db.SaveChangesAsync();
    }

    private SkribblLobbyDdo MapSkribblLobbyToDdo(SkribblLobbyEntity lobby)
    {
        return new SkribblLobbyDdo(
            JsonSerializer.Deserialize<SkribblLobbyStateDdo>(lobby.SkribblDetails) ??
            throw new NullReferenceException("lobby could not be parsed"),
            MapSkribblLobbyTypoSettingsToDdo(lobby)
        );
    }

    private SkribblLobbyTypoMemberDdo MapSkribblLobbyTypoMemberToDdo(SkribblOnlinePlayerEntity player, MemberDdo member)
    {
        var sceneInv = InventoryHelper.ParseSceneInventory(member.Scenes);
        var spriteInv =
            InventoryHelper.ParseActiveSlotsFromInventory(member.Sprites, member.RainbowSprites);

        return new SkribblLobbyTypoMemberDdo(
            player.Login,
            player.LobbyPlayerId,
            player.OwnershipClaim,
            member.Bubbles,
            member.PatronEmoji,
            sceneInv.ActiveId,
            sceneInv.ActiveShift,
            spriteInv
        );
    }

    private SkribblLobbyTypoSettingsDdo MapSkribblLobbyTypoSettingsToDdo(SkribblLobbyEntity lobby)
    {
        return new SkribblLobbyTypoSettingsDdo(
            lobby.LobbyId,
            lobby.Description,
            lobby.WhitelistAllowedServers,
            lobby.AllowedServers.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList(),
            lobby.LobbyOwnershipClaim,
            DateTimeOffset.FromUnixTimeMilliseconds(lobby.FirstSeen),
            DateTimeOffset.FromUnixTimeMilliseconds(lobby.LastUpdated)
        );
    }
}