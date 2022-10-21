using Microsoft.Extensions.DependencyInjection;

namespace Doulex.DistributedCache.DependencyInjection;

public static class DoulexDistributedCacheServiceExtensions
{
    /// <summary>
    /// Adds the thingsboard client sdk api services. 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddDoulexDistributedCache(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        AddService(services);
        return services;
    }

    private static void AddService(IServiceCollection services)
    {
        services.AddTransient(typeof(IObjectCache<>), typeof(ObjectCache<>));
        services.AddTransient<IObjectSerializer, NewtonsoftJsonObjectSerializer>();
    }
}
