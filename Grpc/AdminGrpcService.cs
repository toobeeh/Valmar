using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Domain.Implementation;

namespace Valmar.Grpc;

public class AdminGrpcService(
    ILogger<AdminGrpcService> logger, 
    IMapper mapper,
    IAdminDomainService adminService) : Admin.AdminBase 
{
    public override async Task<Empty> UpdateMemberFlags(UpdateMemberFlagsRequest request, ServerCallContext context)
    {
        logger.LogTrace("UpdateMemberFlags(request={request})", request);

        await adminService.SetPermissionFlag(request.MemberIds, request.FlagId, request.State, request.InvertOthers);
        return new Empty();
    }

    public override async Task<Empty> CreateBubbleTraces(Empty request, ServerCallContext context)
    {
        logger.LogTrace("CreateBubbleTraces(empty)");
        await adminService.CreateBubbleTraces();
        return new Empty();
    }

    public override async Task<Empty> ReevaluateDropChunks(Empty request, ServerCallContext context)
    {
        logger.LogTrace("ReevaluateDropChunks(empty)");
        await adminService.ReevaluateDropChunks();
        return new Empty();
    }
}