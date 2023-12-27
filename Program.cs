using System.Reflection;
using Calzolari.Grpc.AspNetCore.Validation;
using Valmar.Database;
using Valmar.Domain;
using Valmar.Grpc;
using Valmar.Mapper;
using Valmar.Validator.Scenes;

namespace Valmar;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc(options => options.EnableMessageValidation());
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
        services.AddTransient<ScenesDomainService>();
    }

    private static void RegisterGrpcServices(IEndpointRouteBuilder app)
    {
        app.MapGrpcService<ScenesGrpcService>();
    }

    private static void RegisterMapperProfiles(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(SceneMapperProfile));
    }
}