using Microsoft.Extensions.Caching.Distributed;

namespace Doulex.DistributedCache;

public interface IObjectCache<T>
{
    Task<T?> GetAsync(object id, CancellationToken cancel = default);

    Task SetAsync(object id, T obj, CancellationToken cancel = default);
    
    Task SetAsync(object id, T obj, Action<DistributedCacheEntryOptions> optionsCallback, CancellationToken cancel = default);

    Task SetAsync(object id, T obj, DistributedCacheEntryOptions options, CancellationToken cancel = default);

    Task<T?> GetOrAddAsync(object id, Func<Task<T?>> valueFactory, CancellationToken cancel = default);

    Task<T?> GetOrAddAsync(object id, Func<Task<T?>> valueFactory, Action<DistributedCacheEntryOptions> optionsCallback, CancellationToken cancel = default);
    
    Task<T?> GetOrAddAsync(object id, Func<Task<T?>> valueFactory, DistributedCacheEntryOptions options, CancellationToken cancel = default);


    Task RemoveAsync(object id, CancellationToken cancel = default);

    string CacheKeyPrefix { get; set; }
}
