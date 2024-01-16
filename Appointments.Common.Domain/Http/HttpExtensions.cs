using Appointments.Common.Domain.Json;

namespace Appointments.Common.Domain.Http;

public static class HttpExtensions
{
    public static async Task<T> DeserializeJsonAsync<T>(this HttpContent content)
        where T : class
    {
        var json = await content.ReadAsStringAsync();
        return json.DeserializeJson<T>();
    }
}
