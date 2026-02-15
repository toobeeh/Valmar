using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes.Param;
using tobeh.Valmar.Server.Util;
using tobeh.Valmar.Server.Util.NChunkTree.Drops;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class AdminDomainService(
    ILogger<AdminDomainService> logger,
    PalantirContext db,
    IMembersDomainService membersService,
    DropChunkTreeProvider dropChunks) : IAdminDomainService
{
    public Task ReevaluateDropChunks()
    {
        logger.LogTrace("ReevaluateDropChunks()");

        var tree = dropChunks.GetTree();
        dropChunks.RepartitionTree(tree);
        return Task.CompletedTask;
    }

    public async Task WriteOnlineItems(List<OnlineItemDdo> items)
    {
        logger.LogTrace("WriteOnlineItems(items={items})", items);

        var date = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var entities = items.Select(item => new OnlineItemEntity
        {
            Date = Convert.ToInt32(date),
            ItemType = item.ItemType,
            Slot = item.Slot,
            ItemId = item.ItemId,
            LobbyKey = item.LobbyKey,
            LobbyPlayerId = item.LobbyPlayerId
        });

        /* remove old sprites, scenes and slots from lobby players that are now being updated, or are duplicates */
        var duplicates = (await db.OnlineItems.ToListAsync())
            .Where(item =>
                items.Any(i =>
                    i.ItemType == item.ItemType && i.Slot == item.Slot && i.ItemId == item.ItemId
                    && i.LobbyPlayerId == item.LobbyPlayerId && i.LobbyKey == item.LobbyKey
                ) ||
                ((item.ItemType == "sprite" || item.ItemType == "scene" || item.ItemType == "sceneTheme" ||
                  item.ItemType == "shift")
                 && items.Any(i => i.LobbyKey == item.LobbyKey && i.LobbyPlayerId == item.LobbyPlayerId)));

        db.OnlineItems.RemoveRange(duplicates);
        await db.SaveChangesAsync();

        await db.OnlineItems.AddRangeAsync(entities);
        await db.SaveChangesAsync();
    }

    public async Task<List<OnlineItemDdo>> GetAllOnlineItems()
    {
        logger.LogTrace("GetAllOnlineItems()");

        var nowSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var items = await db.OnlineItems.ToListAsync(); /* outdated are deleted by schedule over clearvolatiledata */
        return items
            .Select(item => new OnlineItemDdo(item.ItemType, item.Slot, Convert.ToInt32(item.ItemId), item.LobbyKey,
                item.LobbyPlayerId))
            .ToList();
    }

    public async Task IncrementMemberBubbles(IList<int> userLogins)
    {
        logger.LogTrace("IncrementMemberBubbles(userIds={userLogins})", userLogins);

        var members = db.Members.Where(member => userLogins.Contains(member.Login));
        foreach (var memberEntity in members)
        {
            memberEntity.Bubbles++;
        }

        db.UpdateRange(members);
        await db.SaveChangesAsync();
    }

    public async Task<int> CreateBubbleTraces()
    {
        logger.LogTrace("CreateBubbleTraces()");

        var memberStats = await db.Members.Select(member => new { member.Bubbles, member.Login }).ToListAsync();
        var date = BubbleHelper.FormatTraceTimestamp(DateTimeOffset.UtcNow);

        db.BubbleTraces.RemoveRange(db.BubbleTraces.Where(trace => trace.Date == date));
        await db.SaveChangesAsync();
        var maxId = await db.BubbleTraces.Select(trace => trace.Id).MaxAsync();

        var traces = memberStats.Select(member => new BubbleTraceEntity
        {
            Login = member.Login,
            Date = date,
            Bubbles = member.Bubbles,
            Id = ++maxId
        }).ToList();

        var previousDate = BubbleHelper.FormatTraceTimestamp(DateTimeOffset.Now.AddDays(-1));
        var previousTraces = await db.BubbleTraces
            .Where(trace => trace.Date == previousDate)
            .ToListAsync();

        var changedTraces = traces
            .Where(trace => !previousTraces.Any(prev => prev.Login == trace.Login && prev.Bubbles == trace.Bubbles))
            .ToList();

        await db.BubbleTraces.AddRangeAsync(traces);
        await db.SaveChangesAsync();
        return changedTraces.Count;
    }

    public async Task ClearVolatileData()
    {
        logger.LogTrace("ClearVolatileData()");

        var now = DateTimeOffset.UtcNow;

        var outdatedItems = db.OnlineItems
            .Where(item =>
                item.ItemType != "award" && item.Date < now.AddSeconds(-30).ToUnixTimeSeconds() ||
                item.ItemType == "award" && item.Date < now.AddDays(-2).ToUnixTimeSeconds());

        // Todo check if they are cleared somewhere else as well?
        var outdatedLobbies = db.SkribblLobbies
            .Where(lobby =>
                lobby.LastUpdated < now.AddDays(-1).ToUnixTimeSeconds() &&
                !db.SkribblOnlinePlayers.Any(player =>
                    player.LobbyId == lobby.LobbyId)); // where no player references the lobby

        db.OnlineItems.RemoveRange(outdatedItems);
        db.SkribblLobbies.RemoveRange(outdatedLobbies);

        await db.SaveChangesAsync();
    }

    public async Task SetPermissionFlag(IList<long> userIds, int flag, bool state, bool toggleOthers)
    {
        logger.LogTrace("SetPermissionFlag(userIds={userIds}, flag={flag}, state={state}, toggleOthers={toggleOthers})",
            userIds, flag, state, toggleOthers);

        var memberLogins =
            (await membersService.GetMemberInfosFromDiscordIds(userIds.ToList(), [])).Select(m => m.UserLogin);

        var positives = await db.Members.Where(m => memberLogins.Contains(m.Login.ToString())).ToListAsync();
        var negatives = toggleOthers
            ? await db.Members.Where(m => !memberLogins.Contains(m.Login.ToString())).ToListAsync()
            : [];
        var updates = new List<MemberEntity>();

        foreach (var member in positives)
        {
            var newFlag = RecalculateFlag(member.Flag, flag, state);
            if (newFlag == member.Flag) continue;
            member.Flag = newFlag;
            updates.Add(member);
        }

        foreach (var member in negatives)
        {
            var newFlag = RecalculateFlag(member.Flag, flag, !state);
            if (newFlag == member.Flag) continue;
            member.Flag = newFlag;
            updates.Add(member);
        }

        db.UpdateRange(updates);

        await db.SaveChangesAsync();
    }

    public async Task<List<int>> GetTemporaryPatronLogins()
    {
        logger.LogTrace("GetTemporaryPatronLogins()");

        var patrons = await db.TemporaryPatrons
            .Select(patron => patron.Login)
            .ToListAsync();

        return patrons;
    }

    private static int RecalculateFlag(int flag, int flagIndex, bool state)
    {
        return state ? flag | (1 << flagIndex) : flag & ~(1 << flagIndex);
    }
}