using Microsoft.EntityFrameworkCore.Design;

namespace tobeh.Valmar.Server;

/**
 * Configures the database scaffolding to add "Entity" to the entity models for better distinction
 */
public class ScaffoldingDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        
        services.AddHandlebarsScaffolding();
        services.AddHandlebarsTransformers(
            entityTypeNameTransformer: x => x + "Entity",
            entityFileNameTransformer: x => x + "Entity",
            constructorTransformer: x => {
                x.PropertyType += "Entity";
                return x;
            },
            navPropertyTransformer: x => {
                x.PropertyType += "Entity";
                return x;
            });
    }
}