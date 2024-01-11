using Appointments.Api.Core.User;
using Appointments.Api.Tenant.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Tenant.Controllers;

[ApiController]
[Route("api/tenant/me")]
[ApiVersion("1.0")]
[Produces("application/json")]
[Authorize(Policy = TenantApiPolicy.Me.Scope)]
public class MeController : ControllerBase
{
    private readonly ISender _sender;

    public MeController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(Name = nameof(GetMyProfile))]
    public async Task<UserProfileResponse> GetMyProfile()
    {
        var accessToken = User.GetAccessToken();
        var user = await _sender.Send(new GetUserByEmailRequest(accessToken.Username).ToApplicationRequest());
        return UserProfileResponse.From(user, tenantId: accessToken.TenantId);
    }

    [HttpPut(Name = nameof(UpdateMyProfile))]
    public async Task<NoContentResult> UpdateMyProfile([FromBody] UpdateUserProfileRequest request)
    {
        var accessToken = User.GetAccessToken();
        var user = await _sender.Send(new GetUserByEmailRequest(accessToken.Username).ToApplicationRequest());
        await _sender.Send(request.ToApplicationRequest(user.Id, accessToken.Username));
        return NoContent();
    }
}
