using System.Globalization;
using Calzolari.Grpc.AspNetCore.Validation;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Prometheus;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Implementation;
using tobeh.Valmar.Server.Domain.Implementation.Bubbles;
using tobeh.Valmar.Server.Domain.Implementation.Drops;
using tobeh.Valmar.Server.Grpc;
using tobeh.Valmar.Server.Grpc.Interceptors;
using tobeh.Valmar.Server.Mappers;
using tobeh.Valmar.Server.Util.NChunkTree;
using tobeh.Valmar.Server.Util.NChunkTree.Bubbles;
using tobeh.Valmar.Server.Util.NChunkTree.Drops;

namespace tobeh.Valmar.Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        var builder = WebApplication.CreateBuilder(args);

        // configure kestrel
        builder.WebHost.ConfigureKestrel(options =>
        {
            // Setup a HTTP/2 endpoint without TLS.
            options.ListenAnyIP(builder.Configuration.GetRequiredSection("Grpc").GetValue<int>("HostPort"),
                o => o.Protocols = HttpProtocols.Http2);

            // setup prometheus endpoint
            options.ListenAnyIP(builder.Configuration.GetRequiredSection("Prometheus").GetValue<int>("HostPort"),
                o => o.Protocols = HttpProtocols.Http1);
        });

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

        // initialize drop & bubble chunks to prevent inconsistency with first requests
        var drops = app.Services.GetRequiredService<DropChunkTreeProvider>();
        var bubbles = app.Services.GetRequiredService<BubbleChunkTreeProvider>();
        Task.WaitAll(
            Task.Run(() => drops.RepartitionTree(drops.GetTree())),
            Task.Run(() => bubbles.RepartitionTree(bubbles.GetTree()))
        );

        RegisterGrpcServices(app);

        // use prometheus
        app.UseRouting();
        app.UseGrpcMetrics();
        app.MapMetrics();

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
        services.AddScoped<ICardDomainService, CardDomainService>();
        services.AddScoped<IWorkersDomainService, WorkersDomainService>();
        services.AddScoped<ICloudDomainService, CloudDomainService>();
        services.AddScoped<IAnnouncementsDomainService, AnnouncementsDomainService>();
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
        app.MapGrpcService<CardGrpcService>();
        app.MapGrpcService<WorkersGrpcService>();
        app.MapGrpcService<CloudGrpcService>();
        app.MapGrpcService<AnnouncementsGrpcService>();
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
            typeof(CardMapperProfile),
            typeof(WorkerMapperProfile),
            typeof(CloudMapperProfile),
            typeof(AnnouncementsMapperProfile),
            typeof(DropMapperProfile),
            typeof(AwardMapperProfile));
    }

    private static void RegisterDropChunkAbstraction(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.Configure<DropChunkConfiguration>(builder.Configuration.GetSection("DropChunk"));
        services.AddSingleton<DropChunkTreeProvider>();
        services.AddScoped<PersistentDropChunk>();
        services.AddScoped<CachedDropChunk>();
        services.AddScoped<NChunkTreeNodeContext>(c =>
            throw new InvalidOperationException("Context cannot be created from DI"));
    }

    private static void RegisterBubbleChunkAbstraction(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.Configure<BubbleChunkConfiguration>(builder.Configuration.GetSection("BubbleChunk"));
        services.AddSingleton<BubbleChunkTreeProvider>();
        services.AddScoped<PersistentBubbleChunk>();
        services.AddScoped<CachedBubbleChunk>();
        services.AddScoped<NChunkTreeNodeContext>(c =>
            throw new InvalidOperationException("Context cannot be created from DI"));
    }
}