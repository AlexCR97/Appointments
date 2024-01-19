using Appointments.Common.Domain.Json;

namespace Appointments.Common.Domain.Http;

public sealed class HttpRequestMessageBuilder
{
    private readonly HttpRequestMessage _request;

    public HttpRequestMessageBuilder(HttpMethod method, string url)
    {
        _request = new HttpRequestMessage(method, url);
    }

    public HttpRequestMessageBuilder WithAccessToken(string accessToken)
    {
        _request.Headers.Add("Authorization", $"Bearer {accessToken}");
        return this;
    }

    public HttpRequestMessageBuilder WithJsonContent<T>(T obj)
        where T : class
    {
        _request.Content = obj.ToJsonContent();
        return this;
    }

    public HttpRequestMessage Build()
    {
        return _request;
    }
}
