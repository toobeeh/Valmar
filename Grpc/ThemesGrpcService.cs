using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Grpc.Utils;

namespace Valmar.Grpc;

public class ThemesGrpcService(
    ILogger<ThemesGrpcService> logger, 
    IMapper mapper,
    IThemesDomainService themesService) : Themes.ThemesBase 
{

    public override async Task GetPublishedThemes(Empty request, IServerStreamWriter<ThemeListingReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace($"GetPublishedThemes(empty)");

        var themes = await themesService.GetPublishedThemes();
        await responseStream.WriteAllMappedAsync(themes, async theme =>
        {
            var listing = await themesService.GetListingOfTheme(theme);
            return mapper.Map<ThemeListingReply>(listing);
        });
    }

    public override async Task<ThemeDataReply> GetThemeById(GetThemeRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetThemeById(request={request})", request);

        var theme = await themesService.GetThemeById(request.Id, request.IncrementDownloads);
        return mapper.Map<ThemeDataReply>(theme);
    }

    public override async Task<Empty> PublishTheme(PublishThemeRequest request, ServerCallContext context)
    {
        logger.LogTrace("PublishTheme(request={request})", request);
        
        await themesService.PublishTheme(request.Id, request.Owner);
        return new Empty();
    }

    public override async Task<ThemeShareReply> ShareTheme(ShareThemeRequest request, ServerCallContext context)
    {
        logger.LogTrace("ShareTheme(request={request})", request);

        var id = await themesService.ShareTheme(request.ThemeJson);
        return mapper.Map<ThemeShareReply>(id);
    }

    public override async Task<Empty> UpdateTheme(UpdateThemeRequest request, ServerCallContext context)
    {
        logger.LogTrace("UpdateTheme(request={request})", request);
        
        await themesService.UpdateTheme(request.Id, request.NewId);
        return new Empty();
    }
}