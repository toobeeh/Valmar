using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IWorkersDomainService
{
    Task<LobbyBotInstanceEntity> GetUnclaimedInstanceForWorker();
    Task<LobbyBotInstanceEntity> GetInstanceById(int instanceId);

    Task<LobbyBotInstanceEntity> ClaimInstanceForWorker(string workerUlid, int instanceId, string claimUlid,
        string? lastClaimUlid = null);

    Task<LobbyBotOptionEntity> GetAssignedGuildOptions(int instanceId);
    Task<LobbyBotInstanceEntity> AssignInstanceToServer(MemberDdo member, long serverId);
}