using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Implementation;

namespace tobeh.Valmar.Server.Grpc;

public class AwardsGrpcService(
    ILogger<AwardsGrpcService> logger, 
    IMapper mapper,
    IAwardsDomainService awardsService) : Awards.AwardsBase 
{
    public override async Task GetAllAwards(Empty request, IServerStreamWriter<AwardReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace($"GetAllAwards(empty)");
        
        var awards = await awardsService.GetAllAwards();
        foreach (var award in awards)
        {
            await responseStream.WriteAsync(mapper.Map<AwardReply>(award));
        }
    }

    public override async Task<AwardReply> GetAwardById(GetAwardRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetAwardById(request={request})", request);
        
        var award = await awardsService.GetAwardById(request.Id);
        return mapper.Map<AwardReply>(award);
    }
}