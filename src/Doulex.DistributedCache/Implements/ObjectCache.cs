using Microsoft.Extensions.Caching.Distributed;

namespace Doulex.DistributedCache;

public sealed class ObjectCache<T> : IObjectCache<T> where T : class
{
    private readonly IDistributedCache _cache;
    private readonly IObjectSerializer _serializer;

    public ObjectCache(IDistributedCache cache, IObjectSerializer serializer)
    {
        _cache      = cache;
        _serializer = serializer;
    }

    public async Task<T?> GetAsync(object id, CancellationToken cancel = default)
    {
        var key   = GetCacheKey(id);
        var value = await _cache.GetAsync(key, cancel);
        if (!(value?.Length > 0))
            return null;

        var result = _serializer.Deserialize<T>(value);
        return result;
    }

    public Task SetAsync(object id, T? obj, CancellationToken cancel = default)
    {
        return SetAsync(id, obj, new DistributedCacheEntryOptions(), cancel);
    }

    public Task SetAsync(object id, T? obj, Action<DistributedCacheEntryOptions> optionsCallback, CancellationToken cancel = default)
    {
        var options = new DistributedCacheEntryOptions();
        optionsCallback(options);

        return SetAsync(id, obj, options, cancel);
    }

    public Task SetAsync(object id, T? obj, DistributedCacheEntryOptions options, CancellationToken cancel = default)
    {
        var cacheKey = GetCacheKey(id);
        var value    = obj != null ? _serializer.Serialize(obj) : Array.Empty<byte>();
        return _cache.SetAsync(cacheKey, value, options, cancel);
    }

    public Task<T?> GetOrAddAsync(object id, Func<Task<T?>> valueFactory, CancellationToken cancel = default)
    {
        return GetOrAddAsync(id, valueFactory, new DistributedCacheEntryOptions(), cancel);
    }

    public Task<T?> GetOrAddAsync(
        object                               id,
        Func<Task<T?>>                       valueFactory,
        Action<DistributedCacheEntryOptions> optionsCallback,
        CancellationToken                    cancel = default)
    {
        var options = new DistributedCacheEntryOptions();
        optionsCallback?.Invoke(options);

        return GetOrAddAsync(id, valueFactory, options, cancel);
    }

    public async Task<T?> GetOrAddAsync(object id, Func<Task<T?>> valueFactory, DistributedCacheEntryOptions options, CancellationToken cancel)
    {
        var cachedValue = await GetAsync(id, cancel);
        if (cachedValue != null)
            return cachedValue;

        var newValue = await valueFactory();
        if (newValue == null)
            return null;

        await SetAsync(id, newValue, options, cancel);
        return newValue;
    }

    public async Task RemoveAsync(object id, CancellationToken cancel)
    {
        var cacheKey = GetCacheKey(id);
        await _cache.RemoveAsync(cacheKey, cancel);
    }

    public string CacheKeyPrefix { get; set; } = $"{typeof(T).Name}";

    private string GetCacheKey(object id)
    {
        return $"{CacheKeyPrefix}-{id}";
    }
}
