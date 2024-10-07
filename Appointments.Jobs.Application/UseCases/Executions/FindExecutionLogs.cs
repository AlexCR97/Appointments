using Appointments.Common.Application;
using Appointments.Common.Domain.Models;
using Appointments.Jobs.Domain.Executions;
using FluentValidation;
using MediatR;

namespace Appointments.Jobs.Application.UseCases.Executions;

public sealed record FindExecutionLogsRequest : FindRequest<ExecutionLog>
{
    public Guid? ExecutionId { get; }

    public FindExecutionLogsRequest(int PageIndex, int PageSize, string? Sort, string? Filter, Guid? executionId) : base(PageIndex, PageSize, Sort, Filter)
    {
        ExecutionId = executionId;
    }
}

internal sealed class FindExecutionLogsRequestHandler : IRequestHandler<FindExecutionLogsRequest, PagedResult<ExecutionLog>>
{
    private readonly IExecutionLogRepository _executionLogRepository;

    public FindExecutionLogsRequestHandler(IExecutionLogRepository executionLogRepository)
    {
        _executionLogRepository = executionLogRepository;
    }

    public async Task<PagedResult<ExecutionLog>> Handle(FindExecutionLogsRequest request, CancellationToken cancellationToken)
    {
        new FindRequestValidator<ExecutionLog>().ValidateAndThrow(request);

        return await _executionLogRepository.FindAsync(
            request.PageIndex,
            request.PageSize,
            request.ExecutionId);
    }
}
