using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;

namespace tobeh.Valmar.Server.Grpc;

public class AwardsGrpcService(
    ILogger<AwardsGrpcService> logger,
    IMapper mapper,
    IAwardsDomainService awardsService) : Awards.AwardsBase
{
    public override async Task GetAllAwards(Empty request, IServerStreamWriter<AwardReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace($"GetAllAwards(empty)");

        var awards = await awardsService.GetAllAwards();
        foreach (var award in awards)
        {
            await responseStream.WriteAsync(mapper.Map<AwardReply>(award));
        }
    }

    public override async Task<AwardReply> CreateAward(CreateAwardRequest request, ServerCallContext context)
    {
        logger.LogTrace("CreateAward(request={request})", request);

        var award = await awardsService.CreateAward(request.Name, mapper.Map<int>(request.Rarity), request.Url,
            request.Description);
        return mapper.Map<AwardReply>(award);
    }

    public override async Task<AwardReply> GetAwardById(GetAwardRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetAwardById(request={request})", request);

        var award = await awardsService.GetAwardById(request.Id);
        return mapper.Map<AwardReply>(award);
    }
}