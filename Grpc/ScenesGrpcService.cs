using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Domain.Implementation;
using Valmar.Grpc.Utils;
using Valmar.Util;
using Status = Grpc.Core.Status;

namespace Valmar.Grpc;

public class ScenesGrpcService(
    ILogger<ScenesGrpcService> logger, 
    IMapper mapper,
    IScenesDomainService scenesService) : Scenes.ScenesBase 
{
    public override async Task GetAllScenes(Empty request, IServerStreamWriter<SceneReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace($"GetAllScenes(empty)");
        
        var scenes = await scenesService.GetAllScenes();
        foreach (var scene in scenes)
        {
            await responseStream.WriteAsync(mapper.Map<SceneReply>(scene));
        }
    }

    public override async Task<SceneReply> GetSceneById(GetSceneRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetSceneById(request={request})", request);
        
        var scene = await scenesService.GetSceneById(request.Id);
        return mapper.Map<SceneReply>(scene);
    }

    public override async Task GetSceneRanking(Empty request, IServerStreamWriter<SceneRankingReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetSceneRanking(request={request})", request);

        var ranking = await scenesService.GetSpriteRanking();
        await responseStream.WriteAllMappedAsync(ranking, mapper.Map<SceneRankingReply>);
    }

    public override Task<EventScenePriceReply> GetEventScenePrice(GetEventScenePriceRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetEventScenePrice(request={request})", request);
        
        var price = EventHelper.GetEventScenePrice(request.EventDayLength);
        return Task.FromResult(new EventScenePriceReply { Price = price});
    }
}