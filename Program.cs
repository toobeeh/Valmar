using Calzolari.Grpc.AspNetCore.Validation;
using Valmar.Database;
using Valmar.Domain;
using Valmar.Domain.Implementation;
using Valmar.Domain.Implementation.Drops;
using Valmar.Grpc;
using Valmar.Grpc.Interceptors;
using Valmar.Mappers;
using Valmar.Util.NChunkTree;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar;

public class Program
{
    public static async Task Main(string[] args)
    {
        
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

        // Register routes and start app
        var app = builder.Build();
        
        // initialize drops
        var drops = app.Services.GetRequiredService<DropChunkTreeProvider>();
        drops.RepartitionTree(drops.GetTree());
        
        RegisterGrpcServices(app);
        app.Run();
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
        app.MapGrpcService<DropsGrpcService>();
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
}