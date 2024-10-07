using Appointments.Common.Domain.Enumerables;
using Appointments.Common.Domain.Models;
using Appointments.Jobs.Application.UseCases.Executions;
using Appointments.Jobs.Domain.Executions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace Appointments.Jobs.Infrastructure.Elasticsearch.Executions;

internal sealed class ExecutionLogRepository : IExecutionLogRepository
{
    private readonly ElasticsearchClient _client;

    public ExecutionLogRepository(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task<ExecutionLog> CreateAsync(ExecutionLog entity)
    {
        var document = entity.ToDocument();
        var response = await _client.IndexAsync(document, ExecutionLogDocument.IndexName);
        response.EnsureValidResponse();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _client.DeleteAsync(ExecutionLogDocument.IndexName, id);
        response.EnsureValidResponse();
    }

    public async Task<PagedResult<ExecutionLog>> FindAsync(int pageIndex, int pageSize, Guid? executionId = null)
    {
        var response = await _client.SearchAsync<ExecutionLogDocument>(new SearchRequest
        {
            From = pageIndex * pageSize,
            Size = pageSize,
            QueryLuceneSyntax = new LuceneQueryBuilder()
                .And(executionId is null ? null : @$"executionId:""{executionId.Value}""")
                .Build(),
        });

        response.EnsureValidResponse();

        return new PagedResult<ExecutionLog>(
            pageIndex,
            pageSize,
            response.Documents.Count, // TODO Set correct TotalCount
            response.Documents
                .Select(x => x.ToEntity())
                .ToList());
    }

    public Task<PagedResult<ExecutionLog>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        throw new NotImplementedException();
    }

    public Task<ExecutionLog> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ExecutionLog?> GetOrDefaultAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(ExecutionLog entity)
    {
        throw new NotImplementedException();
    }
}
