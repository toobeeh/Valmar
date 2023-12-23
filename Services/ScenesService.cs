using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Valmar.Database;

namespace Valmar.Services;

public class ScenesService : Scenes.ScenesBase
{
    private readonly ILogger<ScenesService> _logger;
    private readonly PalantirContext _db;

    public ScenesService(ILogger<ScenesService> logger, PalantirContext db)
    {
        _logger = logger;
        _db = db;
    }

    public override async Task GetAllScenes(Empty request, IServerStreamWriter<SceneReply> responseStream, ServerCallContext context)
    {
        var scenes = await _db.Scenes.ToListAsync();
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