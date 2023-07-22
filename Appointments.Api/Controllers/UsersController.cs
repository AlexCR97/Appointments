using Appointments.Application.Requests.Users;
using Appointments.Application.Requests.Users.Login;
using Appointments.Application.Requests.Users.SignUp;
using Appointments.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-up/email", Name = nameof(SignUpWithEmail))]
    public async Task<ActionResult> SignUpWithEmail([FromBody] SignUpWithEmailRequest request)
    {
        var userId = await _mediator.Send(request);

        return CreatedAtRoute(
            nameof(GetUserProfile),
            new { userId },
            new { userId });
    }

    [HttpPost("login/email", Name = nameof(LoginWithEmail))]
    public async Task<ActionResult> LoginWithEmail([FromBody] LoginWithEmailRequest request)
    {
        var oAuthToken = await _mediator.Send(request);
        return Ok(oAuthToken);
    }

    [HttpGet("{userId}/profile", Name = nameof(GetUserProfile))]
    public async Task<UserProfile> GetUserProfile([FromRoute] Guid userId)
    {
        return await _mediator.Send(new GetUserProfileRequest(userId));
    }
}
