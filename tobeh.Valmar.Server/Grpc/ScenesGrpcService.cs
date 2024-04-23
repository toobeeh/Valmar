using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Util;
using tobeh.Valmar.Server.Grpc.Utils;

namespace tobeh.Valmar.Server.Grpc;

public class ScenesGrpcService(
    ILogger<ScenesGrpcService> logger, 
    IMapper mapper,
    IEventsDomainService eventsService,
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

        var ranking = await scenesService.GetSceneRanking();
        await responseStream.WriteAllMappedAsync(ranking, mapper.Map<SceneRankingReply>);
    }

    public override async Task<EventSceneReply> GetEventScene(GetEventSceneRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetEventScene(request={request})", request);
        
        var evt = await eventsService.GetEventById(request.EventId);
        var scene = await scenesService.GetSceneByEventId(evt.Id);
        var price = EventHelper.GetEventScenePrice(evt.Length);
        return new EventSceneReply { Price = price, Scene = mapper.Map<SceneReply>(scene)};
    }
}