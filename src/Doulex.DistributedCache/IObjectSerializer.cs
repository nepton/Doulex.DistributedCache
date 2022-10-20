namespace Doulex.DistributedCache;

/// <summary>
/// 
/// </summary>
public interface IObjectSerializer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cachedValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T? Deserialize<T>(byte[] cachedValue) where T : class;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    byte[] Serialize<T>(T newValue) where T : class;
}
