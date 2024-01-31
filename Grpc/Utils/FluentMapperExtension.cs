using AutoMapper;
using Grpc.Core;

namespace Valmar.Grpc.Utils;

public static class FluentMapperExtension
{
    public static TDestination Map<TSource, TDestination>(this TDestination destination, TSource source, IMapper mapper) // mapper as argument might be improved
    {
        return mapper.Map<TSource, TDestination>(source, destination);
    }
}