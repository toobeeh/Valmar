using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Valmar.Database;

namespace Valmar.Services;

public class ScenesService(ILogger<ScenesService> logger, PalantirContext db) : Scenes.ScenesBase 
{
    public override async Task GetAllScenes(Empty request, IServerStreamWriter<SceneReply> responseStream, ServerCallContext context)
    {
        var scenes = await db.Scenes.ToListAsync();
        foreach (var scene in scenes)
        {
            SceneReply sceneReply = new()
            {
                Artist = scene.Artist,
                EventId = scene.EventId > 0 ? scene.EventId : null,
                Id = scene.Id,
                Name = scene.Name,
                GuessedColor = scene.GuessedColor,
                PrimaryColor = scene.Color
            };
            await responseStream.WriteAsync(sceneReply);
        }
    }
}