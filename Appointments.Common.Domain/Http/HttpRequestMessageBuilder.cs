using Appointments.Common.Domain.Enumerables;
using Appointments.Common.Domain.Json;

namespace Appointments.Common.Domain.Http;

public sealed class HttpRequestMessageBuilder
{
    private readonly HttpMethod _method;
    private readonly string _relativeUrl;
    private readonly Dictionary<string, string> _headers = new();
    private readonly Dictionary<string, string> _query = new();
    private HttpContent? _content;

    public HttpRequestMessageBuilder(HttpMethod method, string relativeUrl)
    {
        _method = method;
        _relativeUrl = relativeUrl;
    }

    public HttpRequestMessageBuilder WithAccessToken(string accessToken)
    {
        _headers["Authorization"] = $"Bearer {accessToken}";
        return this;
    }

    public HttpRequestMessageBuilder WithJsonContent<T>(T obj)
        where T : class
    {
        _content = obj.ToJsonContent();
        return this;
    }

    public HttpRequestMessageBuilder WithQuery(string key, string value)
    {
        _query[key] = value;
        return this;
    }

    public HttpRequestMessage Build()
    {
        var queryString = _query
            .Select(query => $"{query.Key}={query.Value}")
            .JoinToString('$');

        var relativeUrl = $"{_relativeUrl}?{queryString}";

        var request = new HttpRequestMessage(_method, relativeUrl);

        foreach (var header in _headers)
            request.Headers.Add(header.Key, header.Value);

        request.Content = _content;

        return request;
    }
}
