using System.Reflection;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;

namespace tobeh.Valmar.Client.Util;

public static class GrpcClientRegistrationExtensions
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services, string address)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        // Get MethodInfo for the AddGrpcClient method
        var addGrpcClientMethod = typeof(GrpcClientServiceExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m =>
            {
                var methodParams = m.GetParameters();
                if(methodParams.Length != 2) return false;
                    
                return m.Name == "AddGrpcClient" && methodParams[1].ParameterType == typeof(Action<GrpcClientFactoryOptions>) && methodParams[0].ParameterType.IsAssignableTo(typeof(IServiceCollection));
            });

        if (addGrpcClientMethod is null) throw new InvalidOperationException("Reflection could not find AddGrpcClient method");
        
        // get all grpc service clients from the assembly
        var grpcClientTypes = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(ClientBase)));

        // register each client
        foreach (var clientType in grpcClientTypes)
        {
            Action<GrpcClientFactoryOptions> configureClient = opt => opt.Address = new Uri(address);
            addGrpcClientMethod.MakeGenericMethod(new[] { clientType }).Invoke(services, new object[] { services, configureClient });
        }

        return services;
    }
}
