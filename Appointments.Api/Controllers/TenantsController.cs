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

    [HttpPost(Name = nameof(CreateTenant))]
    [Authorize(Roles = $"{TenantPolicy.Roles.Owner},{TenantPolicy.Roles.Admin}")]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request)
    {
        request.CreatedBy = User.GetUsername();
        var tenantId = await _mediator.Send(request);
        return CreatedAtRoute(nameof(GetTenantById), new { tenantId }, new { tenantId });
    }

    [HttpGet(Name = nameof(GetTenants))]
    public async Task<IActionResult> GetTenants(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sort = null,
        [FromQuery] string? filter = null)
    {
        var tenants = await _mediator.Send(new GetTenantsRequest(
            pageIndex,
            pageSize,
            sort,
            filter));

        return Ok(tenants);
    }

    [HttpGet("{tenantId}", Name = nameof(GetTenantById))]
    public async Task<IActionResult> GetTenantById([FromRoute] Guid tenantId)
    {
        var tenant = await _mediator.Send(new GetTenantByIdRequest(tenantId));
        return Ok(tenant);
    }

    [HttpPut("{tenantId}", Name = nameof(UpdateTenant))]
    [Authorize(Roles = $"{TenantPolicy.Roles.Owner},{TenantPolicy.Roles.Admin},{TenantPolicy.Roles.Writer}")]
    public async Task<IActionResult> UpdateTenant(
        [FromRoute] Guid tenantId,
        [FromBody] UpdateTenantRequest request)
    {
        IdMismatchException.ThrowIfMismatch(tenantId, request.Id);
        await _mediator.Send(request);
        return Ok();
    }

    [HttpPut("{tenantId}/logo", Name = nameof(UploadTenantLogo))]
    [Authorize(Roles = $"{TenantPolicy.Roles.Owner},{TenantPolicy.Roles.Admin},{TenantPolicy.Roles.Writer}")]
    public async Task<IActionResult> UploadTenantLogo(
        [FromRoute] Guid tenantId,
        [FromForm] IFormFile image)
    {
        var tenantLogoPath = await _mediator.Send(new UploadTenantLogoRequest(
            tenantId,
            image.FileName,
            image.GetBytes(),
            User.GetUsername()));

        return Ok(new { logoPath = tenantLogoPath });
    }

    [HttpPut("{tenantId}/schedule", Name = nameof(UpdateTenantSchedule))]
    [Authorize(Roles = $"{TenantPolicy.Roles.Owner},{TenantPolicy.Roles.Admin},{TenantPolicy.Roles.Writer}")]
    public async Task<IActionResult> UpdateTenantSchedule(
        [FromRoute] Guid tenantId,
        [FromBody] Requests.UpdateScheduleRequest request)
    {
        IdMismatchException.ThrowIfMismatch(tenantId, request.Id);

        await _mediator.Send(new UpdateTenantScheduleRequest(
            User.GetUsername(),
            request.Id,
            request.WeeklySchedule));

        return Ok();
    }
}
