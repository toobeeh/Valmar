using System.Reflection;
using AutoMapper;
using Calzolari.Grpc.AspNetCore.Validation;
using Grpc.Core.Interceptors;
using Valmar.Database;
using Valmar.Domain;
using Valmar.Domain.Implementation;
using Valmar.Grpc;
using Valmar.Grpc.Interceptors;
using Valmar.Mappers;
using Valmar.Util.NChunkTree;

namespace Valmar;

public class Program
{
    public static void Main(string[] args)
    {
        
        var drops = new List<PastDropEntity>();
        for (var i = 0; i < 10_000_000; i++)
        {
            PastDropEntity drop = new PastDropEntity
            {
                DropId = i,
                EventDropId = i % 10,
                CaughtLobbyKey = "adasd",
                CaughtLobbyPlayerId = "asdasd",
                LeagueWeight = i % 1000,
                ValidFrom = "adasd"
            };
            drops.Add(drop);
        }
        
        var node = new DropChunkTree(8);

        for (int i = 0; i < drops.Count() / 1000; i++)
        {
            node.AddChunk(new DropChunkLeaf(i * 1000, (i * 1000 + 1)));
        }
        
        
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

        // Register routes and start app
        var app = builder.Build();
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
            typeof(AwardMapperProfile));
    }
}