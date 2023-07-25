using Appointments.Api.Exceptions;
using Appointments.Application.Policies;
using Appointments.Application.Requests.BranchOffices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/branch-offices")]
[ApiController]
[Authorize(Policy = BranchOfficePolicy.PolicyName)]
public class BranchOfficesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BranchOfficesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = nameof(GetBranchOffices))]
    public async Task<IActionResult> GetBranchOffices(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sort = null,
        [FromQuery] string? filter = null)
    {
        var branchOffices = await _mediator.Send(new GetBranchOfficesRequest(
            pageIndex,
            pageSize,
            sort,
            filter));

        return Ok(branchOffices);
    }

    [HttpPatch("{branchOfficeId}", Name = nameof(UpdateBranchOffice))]
    [Authorize(Roles = $"{BranchOfficePolicy.Roles.Owner},{BranchOfficePolicy.Roles.Admin}")]
    public async Task<IActionResult> UpdateBranchOffice(
        [FromRoute] Guid branchOfficeId,
        [FromBody] UpdateBranchOfficeRequest request)
    {
        IdMismatchException.ThrowIfMismatch(branchOfficeId, request.Id);
        await _mediator.Send(request);
        return Ok();
    }
}
