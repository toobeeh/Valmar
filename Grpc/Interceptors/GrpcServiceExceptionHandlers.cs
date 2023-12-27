using Grpc.Core;
using Valmar.Domain.Exceptions;

namespace Valmar.Grpc.Interceptors;

public static class GrpcServiceExceptionHandlers
{
    public static RpcException Handle<T>(this Exception exception, ServerCallContext context, ILogger<T> logger) =>
        exception switch
        {
            EntityNotFoundException notFoundException => HandleEntityNotFoundException(notFoundException, logger),
            RpcException rpcException => HandleRpcException(rpcException, logger),
            _ => HandleDefault(exception, context, logger)
        };

    private static RpcException HandleEntityNotFoundException<T>(EntityNotFoundException exception, ILogger<T> logger)
    {
        logger.LogWarning(exception, "An entity requested by rpc service handler could not be found");
        return new RpcException(new Status(StatusCode.NotFound, exception.Message));
    }

    private static RpcException HandleRpcException<T>(RpcException exception, ILogger<T> logger)
    {
        logger.LogWarning(exception, "A rpc exception occured");
        return new RpcException(new Status(exception.StatusCode, exception.Message));
    }

    private static RpcException HandleDefault<T>(Exception exception, ServerCallContext context, ILogger<T> logger)
    {
        logger.LogError(exception, "An unhandled exception occured in the service");
        return new RpcException(new Status(StatusCode.Internal, exception.Message));
    }
}