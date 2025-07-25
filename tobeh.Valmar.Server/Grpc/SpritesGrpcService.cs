using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Grpc.Utils;

namespace tobeh.Valmar.Server.Grpc;

public class SpritesGrpcService(
    ILogger<SpritesGrpcService> logger,
    IMapper mapper,
    ISpritesDomainService spritesService) : Sprites.SpritesBase
{
    public override async Task GetAllSprites(Empty request, IServerStreamWriter<SpriteReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace($"GetAllSprites(empty)");

        var sprites = await spritesService.GetAllSprites();
        foreach (var sprite in sprites)
        {
            await responseStream.WriteAsync(mapper.Map<SpriteReply>(sprite));
        }
    }

    public override async Task<SpriteReply> AddSprite(AddSpriteMessage request, ServerCallContext context)
    {
        logger.LogTrace("AddSprite(request={request})", request);

        var id = await spritesService.AddSprite(request.Name, request.Url, request.Cost, request.EventDropId,
            request.Artist,
            request.IsRainbow);

        var sprite = await spritesService.GetSpriteById(id);
        return mapper.Map<SpriteReply>(sprite);
    }

    public override async Task<SpriteReply> GetSpriteById(GetSpriteRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetSpriteById(request={request})", request);

        var sprite = await spritesService.GetSpriteById(request.Id);
        return mapper.Map<SpriteReply>(sprite);
    }

    public override async Task GetSpriteRanking(Empty request, IServerStreamWriter<SpriteRankingReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetSpriteRanking(request={request})", request);

        var ranking = await spritesService.GetSpriteRanking();
        await responseStream.WriteAllMappedAsync(ranking, mapper.Map<SpriteRankingReply>);
    }

    public override async Task<SpriteReply> UpdateSprite(UpdateSpriteMessage request, ServerCallContext context)
    {
        logger.LogTrace("UpdateSprite(request={request})", request);

        var sprite = await spritesService.UpdateSprite(request.Id, request.Sprite.Name, request.Sprite.Url,
            request.Sprite.Cost,
            request.Sprite.EventDropId, request.Sprite.Artist, request.Sprite.IsRainbow);

        var ddo = await spritesService.GetSpriteById(sprite.Id);
        return mapper.Map<SpriteReply>(ddo);
    }
}