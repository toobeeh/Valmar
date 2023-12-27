using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Status = Grpc.Core.Status;

namespace Valmar.Grpc;

public class ScenesGrpcService(
    ILogger<ScenesGrpcService> logger, 
    IMapper mapper,
    ScenesDomainService scenesService) : Scenes.ScenesBase 
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
        if (scene is null)
        {
            logger.LogInformation("Scene for id {id} was null", request.Id);
            throw new RpcException(new Status(StatusCode.NotFound, $"Scene with id {request.Id} does not exist"));
        }

        return mapper.Map<SceneReply>(scene);
    }
}