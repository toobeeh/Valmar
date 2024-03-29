using System.Text.Json;

namespace Valmar.Domain.Classes.JSON;

public static class ValmarJsonParser
{
    public static TDestination TryParse<TDestination>(string json, ILogger logger) where TDestination : class // TODO improve logger passing
    {
        TDestination? result = null;
        try
        {
            result = JsonSerializer.Deserialize<TDestination>(json, ValmarJsonOptions.JsonSerializerOptions);
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