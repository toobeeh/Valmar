using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Util;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation;

public class AdminDomainService(
    ILogger<AdminDomainService> logger, 
    PalantirContext db,
    IMembersDomainService membersService,
    DropChunkTreeProvider dropChunks) : IAdminDomainService
{
    
    public async Task ReevaluateDropChunks()
    {
        logger.LogTrace("ReevaluateDropChunks()");

        var tree = dropChunks.GetTree();
        dropChunks.RepartitionTree(tree);
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