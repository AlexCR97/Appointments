using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Appointments.Common.Domain.Json;

public static class JsonExtensions
{
    public static T DeserializeJson<T>(this string json)
        where T : class
    {
        return JsonConvert.DeserializeObject<T>(json)
            ?? throw new JsonSerializationException(@$"Could not deserialize json into type ""{typeof(T).Name}"". Json was ""{json}""");
    }

    public static JsonContent ToJsonContent<T>(this T obj)
        where T : class
    {
        return JsonContent.Create(obj);
    }
}
