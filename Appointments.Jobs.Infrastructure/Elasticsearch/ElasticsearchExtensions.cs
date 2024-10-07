using Elastic.Transport.Products.Elasticsearch;

namespace Appointments.Jobs.Infrastructure.Elasticsearch;

internal static class ElasticsearchExtensions
{
    public static void EnsureValidResponse(this ElasticsearchResponse response)
    {
        if (response.IsValidResponse)
            return;

        if (response.TryGetOriginalException(out var exception) && exception is not null)
            throw exception;

        throw new InvalidOperationException("Invalid response from Elasticsearch");
    }
}
