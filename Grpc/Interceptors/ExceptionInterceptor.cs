using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Valmar.Grpc.Interceptors;

public class ExceptionInterceptor(ILogger<ExceptionInterceptor> logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception e)
        {
            throw e.Handle(context, logger);
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception e)
        {
            throw e.Handle(context, logger);
        }
    }
}