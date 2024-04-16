using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes.Param;
using Valmar.Util;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation;

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
        
        var duplicates = (await db.OnlineItems.ToListAsync())
            .Where(item => items.Any(i => 
                i.ItemType == item.ItemType && i.Slot == item.Slot && i.LobbyKey == item.LobbyKey && i.LobbyPlayerId == item.LobbyPlayerId));
        db.OnlineItems.RemoveRange(duplicates);
        await db.SaveChangesAsync();
        
        await db.OnlineItems.AddRangeAsync(entities);
        await db.SaveChangesAsync();
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

    public async Task CreateBubbleTraces()
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

        await db.BubbleTraces.AddRangeAsync(traces);
        await db.SaveChangesAsync();
    }
    
    public async Task ClearVolatileData()
    {
        logger.LogTrace("ClearVolatileData()");

        var now = DateTimeOffset.UtcNow;

        var outdatedReports = db.Reports
            .Where(report =>
                (DateTimeOffset)(object)report.Date < now.AddSeconds(-30));
        var outdatedStatus = db.Statuses
            .Where(status =>
                (DateTimeOffset)(object)status.Date < now.AddSeconds(-10));
        var outdatedSprites = db.OnlineSprites
            .Where(sprite =>
                (DateTimeOffset)(object)sprite.Date < now.AddSeconds(-30));
        var outdatedItems = db.OnlineItems
            .Where(item =>
                item.ItemType != "award" && item.Date < now.AddSeconds(-30).ToUnixTimeSeconds() || 
                item.ItemType == "award" && item.Date < now.AddDays(-2).ToUnixTimeSeconds());
        var outdatedLobbies = db.Lobbies
            .Where(lobby =>
                (long)(object)lobby.LobbyId < now.AddDays(-1).ToUnixTimeMilliseconds() &&
                !db.Statuses.Any(status => status.Status1.Contains(lobby.LobbyId)));         // where no status references the lobby
        
        // TODO ensure that there are no duplicate lobby keys (same key, other ID - fix in future by making key the PK)
        
        db.Reports.RemoveRange(outdatedReports);
        db.Statuses.RemoveRange(outdatedStatus);
        db.OnlineSprites.RemoveRange(outdatedSprites);
        db.OnlineItems.RemoveRange(outdatedItems);
        db.Lobbies.RemoveRange(outdatedLobbies);
        
        await db.SaveChangesAsync();
    }

    public async Task SetPermissionFlag(IList<long> userIds, int flag, bool state, bool toggleOthers)
    {
        logger.LogTrace("SetPermissionFlag(userIds={userIds}, flag={flag}, state={state}, toggleOthers={toggleOthers})", userIds, flag, state, toggleOthers);
        
        var memberLogins = (await membersService.GetMemberInfosFromDiscordIds(userIds.ToList())).Select(m => m.UserLogin);
        
        var positives = await db.Members.Where(m => memberLogins.Contains(m.Login.ToString())).ToListAsync();
        var negatives = toggleOthers ? await db.Members.Where(m => !memberLogins.Contains(m.Login.ToString())).ToListAsync() : [];
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
    
    private static int RecalculateFlag(int flag, int flagIndex, bool state)
    {
        return state ? flag | (1 << flagIndex) : flag & ~(1 << flagIndex);
    }
}