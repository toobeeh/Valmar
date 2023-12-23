using Valmar.Database;
using Valmar.Services;

namespace Valmar;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddDbContext<PalantirContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<ScenesService>();
        app.MapGet("/",
            () =>
                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}