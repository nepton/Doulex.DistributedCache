using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Doulex.DistributedCache;

/// <summary>
/// 分布式缓存扩展方法
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    /// 从缓冲区读取对象, 如果不存在, 执行 valueFactory 生成, 并写入 Cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="distributedCache"></param>
    /// <param name="key"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async Task<T?> GetAsync<T>(
        this IDistributedCache distributedCache,
        string                 key,
        CancellationToken      token = default)
        where T : class
    {
        var valueText = await distributedCache.GetStringAsync(key, token);
        var value     = TryDeserializeObject<T>(valueText);
        return value;
    }

    /// <summary>
    /// 从缓冲区读取对象, 如果不存在, 执行 valueFactory 生成, 并写入 Cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="distributedCache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task SetAsync<T>(
        this IDistributedCache distributedCache,
        string                 key,
        T                      value,
        CancellationToken      token = default)
        where T : class
    {
        return SetAsync(distributedCache, key, value, new DistributedCacheEntryOptions(), token);
    }

    /// <summary>
    /// 从缓冲区读取对象, 如果不存在, 执行 valueFactory 生成, 并写入 Cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="distributedCache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task SetAsync<T>(
        this IDistributedCache       distributedCache,
        string                       key,
        T                            value,
        DistributedCacheEntryOptions options,
        CancellationToken            token = default)
        where T : class
    {
        // 创建 value 对象
        var valueText = JsonConvert.SerializeObject(value);
        return distributedCache.SetStringAsync(key, valueText, options, token);
    }

    /// <summary>
    /// 从缓冲区读取对象, 如果不存在, 执行 valueFactory 生成, 并写入 Cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="distributedCache"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task<T> GetOrAddAsync<T>(
        this IDistributedCache distributedCache,
        string                 key,
        Func<Task<T>>          valueFactory,
        CancellationToken      token = default)
        where T : class?
    {
        return GetOrAddAsync(distributedCache, key, valueFactory, new DistributedCacheEntryOptions(), token);
    }

    /// <summary>
    /// 从缓冲区读取对象, 如果不存在, 执行 valueFactory 生成, 并写入 Cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="distributedCache"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <param name="options"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task<T> GetOrAddAsync<T>(
        this IDistributedCache       distributedCache,
        string                       key,
        Func<Task<T>>                valueFactory,
        DistributedCacheEntryOptions options,
        CancellationToken            token = default)
        where T : class?
    {
        return GetOrAddAsync(distributedCache, key, valueFactory, (_) => options, token);
    }

    /// <summary>
    /// 从缓冲区读取对象, 如果不存在, 执行 valueFactory 生成, 并写入 Cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="distributedCache"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <param name="optionsFunc"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async Task<T> GetOrAddAsync<T>(
        this IDistributedCache                distributedCache,
        string                                key,
        Func<Task<T>>                         valueFactory,
        Func<T, DistributedCacheEntryOptions> optionsFunc,
        CancellationToken                     token = default)
        where T : class?
    {
        var valueText = await distributedCache.GetStringAsync(key, token);
        var value     = TryDeserializeObject<T?>(valueText);

        if (value == null)
        {
            // 创建 value 对象
            value     = await valueFactory();
            valueText = JsonConvert.SerializeObject(value);

            var options = optionsFunc(value);

            // 写入缓存
            await distributedCache.SetStringAsync(key, valueText, options, token);
        }

        return value;
    }

    /// <summary>
    /// 尝试创建对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    private static T? TryDeserializeObject<T>(string value)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        catch
        {
            // ignored
        }

        return default;
    }
}
