using Appointments.Api.Tenant.Exceptions;
using Appointments.Api.Tenant.Models;
using Appointments.Common.Domain.Exceptions;
using Appointments.Core.Application.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Tenant.Controllers;

[ApiController]
[Route("api/tenant/me")]
[ApiVersion("1.0")]
[Authorize(Policy = TenantApiPolicy.Me.Scope)]
public class MeController : ControllerBase
{
    private readonly ISender _sender;

    public MeController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<OkObjectResult> GetMyProfile()
    {
        var userId = GetUserId();
        var tenantId = GetTenantIdOrDefault();
        var user = await _sender.Send(new GetUserRequest(userId));

        if (tenantId is null)
            return Ok(UserProfileResponse.From(user));
        else
            return Ok(MyProfileResponse.From(user, tenantId.Value));
    }

    [HttpPut]
    public async Task<NoContentResult> UpdateMyProfile([FromBody] UpdateUserProfileRequest request)
    {
        Guid userId = GetUserId();
        IdMismatchException.ThrowIfMismatch(userId.ToString(), request.Id.ToString());
        await _sender.Send(request);
        return NoContent();
    }

    private Guid GetUserId()
    {
        var userId = GetClaimValue("id");
        return Guid.Parse(userId);
    }

    private Guid? GetTenantIdOrDefault()
    {
        var tenantId = GetClaimValueOrDefault("tenantId");

        if (tenantId is null)
            return null;

        return Guid.Parse(tenantId);
    }

    private string GetClaimValue(string type)
    {
        return GetClaimValueOrDefault(type)
            ?? throw new DomainException("ClaimMissing", @$"Could not find claim of type ""{type}""");
    }

    private string? GetClaimValueOrDefault(string type)
    {
        var claim = User.FindFirst(type);
        return claim?.Value;
    }
}
