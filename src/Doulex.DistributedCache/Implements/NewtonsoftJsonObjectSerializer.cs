using System.Text;
using Newtonsoft.Json;

namespace Doulex.DistributedCache;

/// <summary>
/// 
/// </summary>
public class NewtonsoftJsonObjectSerializer : IObjectSerializer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cachedValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? Deserialize<T>(byte[] cachedValue) where T : class
    {
        var json = Encoding.UTF8.GetString(cachedValue);
        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public byte[] Serialize<T>(T newValue) where T : class
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(newValue));
    }
}
