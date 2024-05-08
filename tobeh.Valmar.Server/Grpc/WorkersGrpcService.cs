using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;

namespace tobeh.Valmar.Server.Grpc;

public class WorkersGrpcService(
    ILogger<WorkersGrpcService> logger,
    IMapper mapper,
    IMembersDomainService membersDomainService,
    IWorkersDomainService workersDomainService) : Workers.WorkersBase
{
    public override async Task<InstanceDetailsMessage> GetUnclaimedInstance(Empty request, ServerCallContext context)
    {
        logger.LogTrace("GetUnclaimedInstance(request={request})", request);

        var instance = await workersDomainService.GetUnclaimedInstanceForWorker();
        return mapper.Map<InstanceDetailsMessage>(instance);
    }

    public override async Task<InstanceDetailsMessage> ClaimInstance(ClaimInstanceMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("ClaimInstance(request={request})", request);

        var instance = await workersDomainService.ClaimInstanceForWorker(request.WorkerUlid, request.InstanceId,
            request.ClaimUlid, request.LastClaimUlid);
        return mapper.Map<InstanceDetailsMessage>(instance);
    }

    public override async Task<GuildOptionsMessage> GetAssignedGuildOptions(GetAssignedGuildOptionsMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("GetAssignedGuildOptions(request={request})", request);

        var options = await workersDomainService.GetAssignedGuildOptions(request.InstanceId);
        return mapper.Map<GuildOptionsMessage>(options);
    }

    public override async Task<InstanceDetailsMessage> AssignInstanceToServer(AssignInstanceToServerMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("AssignInstanceToServer(request={request})", request);

        var member = await membersDomainService.GetMemberByLogin(request.Login);
        var instance = await workersDomainService.AssignInstanceToServer(member, request.ServerId);

        return mapper.Map<InstanceDetailsMessage>(instance);
    }
}