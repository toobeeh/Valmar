using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Domain.Implementation;
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
}