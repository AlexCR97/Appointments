using Appointments.Api.Tenant.Exceptions;
using Appointments.Api.Tenant.Models;
using Appointments.Core.Application.Requests.Tenants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Tenant.Controllers;

[ApiController]
[Route("api/tenant/tenants")]
[ApiVersion("1.0")]
[Produces("application/json")]
[Authorize(Policy = TenantApiPolicy.Tenants.Scope)]
public class TenantsController : ControllerBase
{
    private readonly ISender _sender;

    public TenantsController(ISender sender)
    {
        _sender = sender;
    }

    #region Tenants

    [HttpGet("{id}", Name = nameof(GetTenant))]
    public async Task<TenantProfileResponse> GetTenant([FromRoute] Guid id)
    {
        var tenant = await _sender.Send(new GetTenantRequest(id));
        return TenantProfileResponse.From(tenant);
    }

    [HttpPut("{id}", Name = nameof(UpdateTenantProfile))]
    public async Task<NoContentResult> UpdateTenantProfile(
        [FromRoute] Guid id,
        [FromBody] UpdateTenantProfileRequest request)
    {
        IdMismatchException.ThrowIfMismatch(id.ToString(), request.Id.ToString());
        await _sender.Send(request);
        return NoContent();
    }

    #endregion
}
