using System.Text.Json;

namespace tobeh.Valmar.Server.Domain.Classes.JSON;

public static class ValmarJsonParser
{
    public static TDestination TryParse<TDestination>(string json, ILogger logger, bool useDefaults = false) where TDestination : class // TODO improve logger passing
    {
        TDestination? result = null;
        try
        {
            result = useDefaults ? JsonSerializer.Deserialize<TDestination>(json) : JsonSerializer.Deserialize<TDestination>(json, ValmarJsonOptions.JsonSerializerOptions);
        }
        catch(Exception e)
        {
            logger.LogError(e, $"Failed to parse {nameof(TDestination)}");
            result = null;
        }
        
        if (result is null)
        {
            throw new NullReferenceException($"Failed to parse {nameof(TDestination)}: \n{result}");
        }
        
        return result;
    }
}