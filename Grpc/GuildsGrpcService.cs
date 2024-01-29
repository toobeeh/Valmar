using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Domain.Implementation;

namespace Valmar.Grpc;

public class GuildsGrpcService(
    ILogger<GuildsGrpcService> logger, 
    IMapper mapper,
    IGuildsDomainService guildsService) : Guilds.GuildsBase 
{

    public override async Task<GuildReply> GetGuildByToken(GetGuildRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetGuildByToken(request={request})", request);

        var details = await guildsService.GetGuildByObserveToken(request.ObserveToken);
        return mapper.Map<GuildReply>(details);
    }
}