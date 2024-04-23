using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace tobeh.Valmar.Server.Domain.Classes.JSON;

public static class ValmarJsonOptions
{
    public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers =
            {
                static typeInfo =>
                {
                    if (typeInfo.Kind != JsonTypeInfoKind.Object)
                        return;

                    foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
                    {
                        // Strip IsRequired constraint from every property.
                        propertyInfo.IsRequired = true;
                    }
                }
            }
        }
    };
}