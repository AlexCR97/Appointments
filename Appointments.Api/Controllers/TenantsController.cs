using Appointments.Api.Exceptions;
using Appointments.Api.Extensions.AspNetCore;
using Appointments.Api.Extensions.Files;
using Appointments.Application.Policies;
using Appointments.Application.Requests.Tenants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/tenants")]
[ApiController]
[Authorize(Policy = TenantPolicy.PolicyName)]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{tenantId}", Name = nameof(GetTenant))]
    public async Task<IActionResult> GetTenant([FromRoute] Guid tenantId)
    {
        var tenant = await _mediator.Send(new GetTenantRequest(tenantId));
        return Ok(tenant);
    }

    [HttpPatch("{tenantId}", Name = nameof(UpdateTenant))]
    [Authorize(Roles = $"{TenantPolicy.Roles.Owner},{TenantPolicy.Roles.Admin}")]
    public async Task<IActionResult> UpdateTenant(
        [FromRoute] Guid tenantId,
        [FromBody] UpdateTenantRequest request)
    {
        IdMismatchException.ThrowIfMismatch(tenantId, request.Id);
        await _mediator.Send(request);
        return Ok();
    }

    [HttpPost("{tenantId}/logo", Name = nameof(UploadTenantLogo))]
    [Authorize(Roles = $"{TenantPolicy.Roles.Owner},{TenantPolicy.Roles.Admin}")]
    public async Task<IActionResult> UploadTenantLogo(
        [FromRoute] Guid tenantId,
        [FromForm] IFormFile image)
    {
        var tenantLogoPath = await _mediator.Send(new UploadLogoRequest(
            tenantId,
            image.FileName,
            image.GetBytes(),
            User.GetUsername()));

        return Ok(new { logoPath = tenantLogoPath });
    }

    [HttpPut("{tenantId}/schedule", Name = nameof(UpdateTenantSchedule))]
    [Authorize(Roles = $"{TenantPolicy.Roles.Owner},{TenantPolicy.Roles.Admin}")]
    public async Task<IActionResult> UpdateTenantSchedule(
        [FromRoute] Guid tenantId,
        [FromBody] Requests.UpdateScheduleRequest request)
    {
        IdMismatchException.ThrowIfMismatch(tenantId, request.Id);

        await _mediator.Send(new UpdateScheduleRequest(
            User.GetUsername(),
            request.Id,
            request.WeeklySchedule));

        return Ok();
    }
}
