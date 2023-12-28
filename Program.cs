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
using Valmar.Validators.Scenes;

namespace Valmar;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionInterceptor>();
            options.EnableDetailedErrors = true;
            options.EnableMessageValidation();
        });
        //builder.Services.AddSingleton<Interceptor, ExceptionInterceptor>();
        builder.Services.AddValidators();
        builder.Services.AddGrpcValidation();
        builder.Services.AddDbContext<PalantirContext>();
        RegisterMapperProfiles(builder.Services);
        RegisterDomainServices(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        RegisterGrpcServices(app);
        app.MapGet("/",
            () =>
                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }

    private static void RegisterDomainServices(IServiceCollection services)
    {
        services.AddScoped<IScenesDomainService, ScenesDomainService>();
        services.AddScoped<ISpritesDomainService, SpritesDomainService>();
        services.AddScoped<IAwardsDomainService, AwardsDomainService>();
        services.AddScoped<IEventsDomainService, EventsDomainService>();
        services.AddScoped<IThemesDomainService, ThemesDomainService>();
    }

    private static void RegisterGrpcServices(IEndpointRouteBuilder app)
    {
        app.MapGrpcService<ScenesGrpcService>();
        app.MapGrpcService<SpritesGrpcService>();
        app.MapGrpcService<AwardsGrpcService>();
        app.MapGrpcService<EventsGrpcService>();
        app.MapGrpcService<ThemesGrpcService>();
    }

    private static void RegisterMapperProfiles(IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(EventMapperProfile),
            typeof(SceneMapperProfile),
            typeof(SpriteMapperProfile),
            typeof(ThemeMapperProfile),
            typeof(AwardMapperProfile));
    }
}