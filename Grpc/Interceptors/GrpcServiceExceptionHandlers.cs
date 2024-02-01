using Grpc.Core;
using Valmar.Domain.Exceptions;

namespace Valmar.Grpc.Interceptors;

public static class GrpcServiceExceptionHandlers
{
    public static RpcException Handle<T>(this Exception exception, ServerCallContext context, ILogger<T> logger) =>
        exception switch
        {
            EntityNotFoundException notFoundException => HandleEntityNotFoundException(notFoundException, logger),
            EntityAlreadyExistsException existsException => HandleEntityAlreadyExistsException(existsException, logger),
            EntityConflictException conflictException => HandleConflictException(conflictException, logger),
            RpcException rpcException => HandleRpcException(rpcException, logger),
            _ => HandleDefault(exception, context, logger)
        };

    private static RpcException HandleEntityNotFoundException<T>(EntityNotFoundException exception, ILogger<T> logger)
    {
        logger.LogWarning(exception, "An entity requested by rpc service handler could not be found");
        return new RpcException(new Status(StatusCode.NotFound, exception.Message));
    }
    
    private static RpcException HandleEntityAlreadyExistsException<T>(EntityAlreadyExistsException exception, ILogger<T> logger)
    {
        logger.LogWarning(exception, "An entity requested to be created already exists");
        return new RpcException(new Status(StatusCode.AlreadyExists, exception.Message));
    }
    
    private static RpcException HandleConflictException<T>(EntityConflictException exception, ILogger<T> logger)
    {
        logger.LogWarning(exception, "Some given arguments are in conflict with the current state");
        return new RpcException(new Status(StatusCode.InvalidArgument, exception.Message));
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