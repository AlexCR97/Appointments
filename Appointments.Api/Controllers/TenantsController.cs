using Appointments.Application.Requests.Tenants;
using Appointments.Domain.Models.Tenants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{tenantId}", Name = nameof(GetTenant))]
    public async Task<TenantModel> GetTenant([FromRoute] Guid tenantId)
    {
        return await _mediator.Send(new GetTenantRequest(tenantId));
    }
}
