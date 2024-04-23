using AutoMapper;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;

namespace tobeh.Valmar.Server.Grpc;

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

    public override async Task<GuildReply> GetGuildByDiscordId(GetGuildByIdMessage request, ServerCallContext context)
    {
        logger.LogTrace("GetGuildByDiscordId(request={request})", request);

        var details = await guildsService.GetGuildByDiscordId(request.DiscordId);
        return mapper.Map<GuildReply>(details);
    }
}