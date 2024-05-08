using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class WorkersDomainService(
    ILogger<WorkersDomainService> logger,
    IMembersDomainService membersDomainService,
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
            throw new EntityNotFoundException($"There is no free bot instance available to claim for servers");
        }

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
                $"Instance {instanceId} is already claimed by another worker, or renewal claim rejected because old claim {lastClaimUlid} did not match {instance.ClaimUlid}.");
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

        var memberClaim = await db.LobbyBotClaims.FirstOrDefaultAsync(entity => entity.InstanceId == instanceId);
        if (memberClaim is null)
        {
            throw new EntityNotFoundException($"No member has claimed instance with id {instanceId}");
        }

        var member = await membersDomainService.GetMemberByLogin(memberClaim.Login);
        if (!member.MappedFlags.Contains(MemberFlagDdo.Patron))
        {
            throw new UserOperationException("Claim of the instance is no longer valid");
        }

        return await GetGuildOptionsByGuildId(memberClaim.GuildId);
    }

    public async Task<LobbyBotOptionEntity> GetGuildOptionsByGuildId(long guildId)
    {
        logger.LogTrace("GetGuildOptionsByGuildId(guildId={guildId})", guildId);

        var options = await db.LobbyBotOptions.FirstOrDefaultAsync(entity => entity.GuildId == guildId);
        if (options is null)
        {
            throw new EntityNotFoundException($"No guild options for id {guildId}");
        }

        return options;
    }

    public async Task<LobbyBotInstanceEntity> AssignInstanceToServer(MemberDdo member, long serverId)
    {
        logger.LogTrace("AssignInstanceToServer(member={member}, serverId={serverId})", member, serverId);

        if (!member.MappedFlags.Contains(MemberFlagDdo.Patron))
        {
            throw new UserOperationException("Member is not a patron and cant assign instances");
        }

        var memberClaim = await db.LobbyBotClaims.FirstOrDefaultAsync(entity => entity.Login == member.Login);
        if (memberClaim is not null && !CanClaimNewInstance(memberClaim))
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
        var otherServerClaim = await db.LobbyBotClaims.FirstOrDefaultAsync(entity => entity.GuildId == serverId);
        if (memberClaim is not null)
        {
            instance = await GetInstanceById(memberClaim.InstanceId);
            newClaim.InstanceId = memberClaim.InstanceId;
        }
        else if (otherServerClaim is not null)
        {
            instance = await GetInstanceById(otherServerClaim.InstanceId);
            newClaim.InstanceId = otherServerClaim.InstanceId;
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
                Invite = invite
            };
            db.LobbyBotOptions.Add(options);
        }

        await db.SaveChangesAsync();

        return instance;
    }

    public async Task<LobbyBotOptionEntity> UpdateGuildOptions(long guildId, string name, string prefix,
        long? channelId = null)
    {
        logger.LogTrace("UpdateGuildOptions(guildId={guildId}, name={name})", guildId, name);

        var options = await db.LobbyBotOptions.FirstOrDefaultAsync(entity => entity.GuildId == guildId);
        if (options is null)
        {
            throw new EntityNotFoundException($"No guild options for id {guildId}");
        }

        options.Name = name;
        options.Prefix = prefix;
        options.ChannelId = channelId;

        db.LobbyBotOptions.Update(options);
        await db.SaveChangesAsync();

        return options;
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