using System.Globalization;
using Calzolari.Grpc.AspNetCore.Validation;
using Valmar.Database;
using Valmar.Domain;
using Valmar.Domain.Implementation;
using Valmar.Domain.Implementation.Bubbles;
using Valmar.Domain.Implementation.Drops;
using Valmar.Grpc;
using Valmar.Grpc.Interceptors;
using Valmar.Mappers;
using Valmar.Util.NChunkTree;
using Valmar.Util.NChunkTree.Bubbles;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar;

public class Program
{
    public static async Task Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionInterceptor>();
            options.EnableDetailedErrors = true;
            options.EnableMessageValidation();
        });
        builder.Services.AddValidators();
        builder.Services.AddGrpcValidation();
        builder.Services.AddDbContext<PalantirContext>();
        RegisterMapperProfiles(builder.Services);
        RegisterDomainServices(builder.Services);
        RegisterDropChunkAbstraction(builder);
        RegisterBubbleChunkAbstraction(builder);

        // Register routes and start app
        var app = builder.Build();
        
        // initialize drop & bubble chunks
        var drops = app.Services.GetRequiredService<DropChunkTreeProvider>();
        var bubbles = app.Services.GetRequiredService<BubbleChunkTreeProvider>();
        Task.WaitAll(
            Task.Run(() => drops.RepartitionTree(drops.GetTree())),
            Task.Run(() => bubbles.RepartitionTree(bubbles.GetTree()))
        );

        RegisterGrpcServices(app);
        await app.RunAsync();
    }

    private static void RegisterDomainServices(IServiceCollection services)
    {
        services.AddScoped<IScenesDomainService, ScenesDomainService>();
        services.AddScoped<ISpritesDomainService, SpritesDomainService>();
        services.AddScoped<IAwardsDomainService, AwardsDomainService>();
        services.AddScoped<IEventsDomainService, EventsDomainService>();
        services.AddScoped<IThemesDomainService, ThemesDomainService>();
        services.AddScoped<IGuildsDomainService, GuildsDomainService>();
        services.AddScoped<ILobbiesDomainService, LobbiesDomainService>();
        services.AddScoped<IMembersDomainService, MembersDomainService>();
        services.AddScoped<IAdminDomainService, AdminDomainService>();
        services.AddScoped<ILeaguesDomainService, LeaguesDomainService>();
        services.AddScoped<IDropsDomainService, DropsDomainService>();
        services.AddScoped<IInventoryDomainService, InventoryDomainService>();
        services.AddScoped<IStatsDomainService, StatsDomainService>();
        services.AddScoped<ISplitsDomainService, SplitsDomainService>();
        services.AddScoped<IOutfitsDomainService, OutfitsDomainService>();
    }

    private static void RegisterGrpcServices(IEndpointRouteBuilder app)
    {
        app.MapGrpcService<ScenesGrpcService>();
        app.MapGrpcService<SpritesGrpcService>();
        app.MapGrpcService<AwardsGrpcService>();
        app.MapGrpcService<EventsGrpcService>();
        app.MapGrpcService<ThemesGrpcService>();
        app.MapGrpcService<GuildsGrpcService>();
        app.MapGrpcService<MembersGrpcService>();
        app.MapGrpcService<LobbiesGrpcService>();
        app.MapGrpcService<LeaguesGrpcService>();
        app.MapGrpcService<AdminGrpcService>();
        app.MapGrpcService<StatsGrpcService>();
        app.MapGrpcService<DropsGrpcService>();
        app.MapGrpcService<InventoryGrpcService>();
        app.MapGrpcService<SplitsGrpcService>();
        app.MapGrpcService<OutfitsGrpcService>();
    }

    private static void RegisterMapperProfiles(IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(EventMapperProfile),
            typeof(LobbyMapperProfile),
            typeof(SceneMapperProfile),
            typeof(SpriteMapperProfile),
            typeof(ThemeMapperProfile),
            typeof(GuildMapperProfile),
            typeof(MemberMapperProfile),
            typeof(AdminMapperProfile),
            typeof(InventoryMapperProfile),
            typeof(BasicMapperProfile),
            typeof(StatMapperProfile),
            typeof(SceneMapperProfile),
            typeof(OutfitMapperProfile),
            typeof(AwardMapperProfile));
    }
    
    private static void RegisterDropChunkAbstraction(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.Configure<DropChunkConfiguration>(builder.Configuration.GetSection("DropChunk"));
        services.AddSingleton<DropChunkTreeProvider>();
        services.AddScoped<PersistentDropChunk>();
        services.AddScoped<CachedDropChunk>();
        services.AddScoped<NChunkTreeNodeContext>(c => throw new InvalidOperationException("Context cannot be created from DI"));
    }
    
    private static void RegisterBubbleChunkAbstraction(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.Configure<BubbleChunkConfiguration>(builder.Configuration.GetSection("BubbleChunk"));
        services.AddSingleton<BubbleChunkTreeProvider>();
        services.AddScoped<PersistentBubbleChunk>();
        services.AddScoped<CachedBubbleChunk>();
        services.AddScoped<NChunkTreeNodeContext>(c => throw new InvalidOperationException("Context cannot be created from DI"));
    }
}