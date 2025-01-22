using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class WorkersDomainService(
    ILogger<WorkersDomainService> logger,
    IMembersDomainService membersDomainService,
    IGuildsDomainService guildsDomainService,
    PalantirContext db) : IWorkersDomainService
{
    public async Task<LobbyBotInstanceEntity> GetUnclaimedInstanceForWorker()
    {
        logger.LogTrace("GetUnclaimedInstanceForWorker()");

        var instances = await db.LobbyBotInstances.ToListAsync();

        var unclaimedInstances = instances.Where(InstanceUnclaimedOrExpired);

        var instance = unclaimedInstances.FirstOrDefault();
        if (instance is null)
        {
            throw new EntityNotFoundException($"There is no free bot instance available to claim for workers");
        }

        return instance;
    }

    public async Task<LobbyBotInstanceEntity> GetUnclaimedInstanceForServer()
    {
        logger.LogTrace("GetUnclaimedInstanceForServer()");

        var instances = await db.LobbyBotInstances.ToListAsync();
        var memberClaims = await db.LobbyBotClaims.ToListAsync();
        var memberPermissions =
            (await membersDomainService.GetMembersByLogin(memberClaims.Select(member => member.Login).ToList()))
            .ToDictionary(member => member.Login, member => member.MappedFlags.Contains(MemberFlagDdo.Patron));
        var validMemberClaims = memberClaims.Where(claim => memberPermissions[claim.Login])
            .Select(claim => claim.InstanceId).ToList();

        var freeInstances = instances.Where(instance => !validMemberClaims.Contains(instance.Id));
        var instance = freeInstances.FirstOrDefault();
        if (instance is null)
        {
            throw new EntityNotFoundException($"There is no free bot instance available to claim for servers", false);
        }

        // remove all claims of this instance that may be left (must be invalid if any, as checked before)
        var danglingClaims = memberClaims.Where(claim => claim.InstanceId == instance.Id);
        db.LobbyBotClaims.RemoveRange(danglingClaims);
        await db.SaveChangesAsync();

        return instance;
    }

    public async Task<LobbyBotInstanceEntity> GetInstanceById(int instanceId)
    {
        logger.LogTrace("GetInstanceById(instanceId={instanceId})", instanceId);

        var instance = await db.LobbyBotInstances.FirstOrDefaultAsync(entity => entity.Id == instanceId);
        if (instance is null)
        {
            throw new EntityNotFoundException($"Bot instance with id {instanceId} not found");
        }

        return instance;
    }

    public async Task<LobbyBotInstanceEntity> ClaimInstanceForWorker(string workerUlid, int instanceId,
        string claimUlid, string? lastClaimUlid = null)
    {
        logger.LogTrace("ClaimInstanceForWorker(workerId={workerId})", workerUlid);

        var instance = await GetInstanceById(instanceId);
        if (!InstanceUnclaimedOrExpired(instance) && lastClaimUlid != instance.ClaimUlid)
        {
            throw new UserOperationException(
                $"Instance {instanceId} is already claimed by another worker, or renewal claim rejected because old claim {lastClaimUlid} did not match {instance.ClaimUlid}.",
                false);
        }

        if (instance.ClaimUlid == claimUlid)
        {
            throw new UserOperationException("New claim ulid cannot be the same as the old one");
        }

        instance.ClaimUlid = claimUlid;
        instance.ClaimedWorkerUlid = workerUlid;

        await db.SaveChangesAsync();

        return instance;
    }

    public async Task<LobbyBotOptionEntity> GetAssignedGuildOptions(int instanceId)
    {
        logger.LogTrace("GetAssignedGuildOptions(instanceId={instanceId})", instanceId);

        var memberClaims = await db.LobbyBotClaims.Where(entity => entity.InstanceId == instanceId).ToListAsync();
        if (memberClaims.Count == 0)
        {
            throw new EntityNotFoundException($"No member has claimed instance with id {instanceId}", false);
        }

        /* find valid claim */
        foreach (var claim in memberClaims)
        {
            var member = await membersDomainService.GetMemberByLogin(claim.Login);
            if (member.MappedFlags.Contains(MemberFlagDdo.Patron))
                return await guildsDomainService.GetGuildOptionsByGuildId(claim.GuildId);
        }

        throw new EntityNotFoundException($"No member with active patronage has claimed instance with id {instanceId}",
            false);
    }

    public async Task<LobbyBotInstanceEntity> AssignInstanceToServer(MemberDdo member, long serverId)
    {
        logger.LogTrace("AssignInstanceToServer(member={member}, serverId={serverId})", member, serverId);

        if (!member.MappedFlags.Any(flag => flag is MemberFlagDdo.Patron or MemberFlagDdo.Admin))
        {
            throw new UserOperationException("Member is not a patron and cant assign instances");
        }

        if (member.NextHomeChooseDate > DateTimeOffset.UtcNow)
        {
            throw new UserOperationException("Member has already claimed an instance in the last 7 days");
        }

        var newClaim = new LobbyBotClaimEntity
        {
            ClaimTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Login = member.Login,
            GuildId = serverId
        };

        LobbyBotInstanceEntity instance;

        // check if user has claimed another server, or server is already chosen
        var memberClaim = await db.LobbyBotClaims.FirstOrDefaultAsync(entity => entity.Login == member.Login);
        var otherServerClaims = await db.LobbyBotClaims.Where(entity => entity.GuildId == serverId).ToListAsync();

        // clean expired server claims
        var expiredClaims = new List<LobbyBotClaimEntity>();
        foreach (var claim in otherServerClaims)
        {
            var memberCheck = await membersDomainService.GetMemberByLogin(claim.Login);
            if (!memberCheck.MappedFlags.Contains(MemberFlagDdo.Patron)) expiredClaims.Add(claim);
        }

        db.LobbyBotClaims.RemoveRange(expiredClaims);
        await db.SaveChangesAsync();
        otherServerClaims = otherServerClaims.Where(claim => !expiredClaims.Contains(claim)).ToList();

        /* if member has already an instance claimed, change associated server or if server has already instance, claim with server id */
        if (memberClaim is not null)
        {
            /* prefer to set instance already in use by other server members */
            var id = otherServerClaims.Count > 0 ? otherServerClaims.First().InstanceId : memberClaim.InstanceId;

            instance = await GetInstanceById(id);
            newClaim.InstanceId = id;
        }
        /* if member not yet claimed any but server has an instance, claim the same */
        else if (otherServerClaims.Count > 0)
        {
            instance = await GetInstanceById(otherServerClaims.First().InstanceId);
            newClaim.InstanceId = instance.Id;
        }
        else
        {
            // assign new bot
            instance = await GetUnclaimedInstanceForServer();
            newClaim.InstanceId = instance.Id;
        }

        // remove old claim
        if (memberClaim is not null)
        {
            db.LobbyBotClaims.Remove(memberClaim);
            await db.SaveChangesAsync();
        }

        // add new claim
        db.LobbyBotClaims.Add(newClaim);

        // if no guild options set, add
        var options = await db.LobbyBotOptions.FirstOrDefaultAsync(entity => entity.GuildId == serverId);
        if (options is null)
        {
            int invite;
            do
            {
                invite = new Random().Next(999999);
            } while (await db.LobbyBotOptions.AnyAsync(entity => entity.Invite == invite));

            options = new LobbyBotOptionEntity
            {
                GuildId = serverId,
                Name = "Server",
                Prefix = ".",
                ChannelId = null,
                Invite = invite,
                ProxyLinks = true,
                ShowInvite = true
            };
            db.LobbyBotOptions.Add(options);
        }

        await db.SaveChangesAsync();

        return instance;
    }

    private static bool CanClaimNewInstance(LobbyBotClaimEntity claim)
    {
        var lastClaim = DateTimeOffset.FromUnixTimeMilliseconds(claim.ClaimTimestamp);
        return DateTimeOffset.UtcNow - lastClaim > TimeSpan.FromDays(7);
    }

    private static bool InstanceUnclaimedOrExpired(LobbyBotInstanceEntity instance)
    {
        if (instance.ClaimUlid is null)
        {
            return true;
        }

        var claimUlid = Ulid.Parse(instance.ClaimUlid);
        return DateTimeOffset.UtcNow - claimUlid.Time > TimeSpan.FromMinutes(1);
    }
}