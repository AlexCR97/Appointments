using Appointments.Api.Management.Models;
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
}
