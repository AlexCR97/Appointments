using Appointments.Api.Management.Models;
using Appointments.Common.Application;
using Appointments.Common.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Management.Controllers;

[ApiController]
[Route("api/management/jobs")]
[ApiVersion("1.0")]
[Produces("application/json")]
// TODO Set auth policies
//[Authorize(Policy = TenantApiPolicy.Me.Scope)]
[AllowAnonymous]
public class JobsController : ControllerBase
{
    private readonly ISender _sender;

    public JobsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(Name = nameof(CreateJob))]
    public async Task<JobCreatedResponse> CreateJob(
        [FromBody] CreateJobRequest request)
    {
        var applicationRequest = request.ToApplicationRequest("Unknown");
        var jobId = await _sender.Send(applicationRequest);
        return new JobCreatedResponse(jobId);
    }

    [HttpPost("{jobId}/run", Name = nameof(RunJob))]
    public async Task<ExecutionQueuedResponse> RunJob(
        [FromRoute] Guid jobId,
        [FromBody] RunJobRequest request)
    {
        var applicationRequest = request.ToApplicationRequest("Unknown", jobId);
        var response = await _sender.Send(applicationRequest);
        return new ExecutionQueuedResponse(response.ExecutionId);
    }

    [HttpPost("{jobId}/executions/{executionId}/cancel", Name = nameof(CancelExecution))]
    public async Task<AcceptedResult> CancelExecution(
        [FromRoute] Guid jobId,
        [FromRoute] Guid executionId)
    {
        // TODO Verify that the execution belongs to the job

        await _sender.Send(new Jobs.Application.UseCases.Executions.CancelExecutionRequest(
            "Unknown",
            executionId));

        return Accepted();
    }

    [HttpGet("{jobId}/executions/{executionId}/logs", Name = nameof(FindExecutionLogs))]
    public async Task<PagedResult<ExecutionLogResponse>> FindExecutionLogs(
        [FromRoute] Guid jobId,
        [FromRoute] Guid executionId,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = FindRequest.PageSize.Default,
        [FromQuery] string? sort = null,
        [FromQuery] string? filter = null)
    {
        // TODO Verify that the execution belongs to the job

        var pagedResult = await _sender.Send(new Jobs.Application.UseCases.Executions.FindExecutionLogsRequest(
            pageIndex,
            pageSize,
            sort,
            filter,
            executionId));

        return pagedResult.Map(ExecutionLogResponse.From);
    }
}
