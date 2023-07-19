using Appointments.Application.Requests.Users;
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

    [HttpGet("{userId}", Name = nameof(GetUserProfile))]
    public async Task<UserProfile> GetUserProfile([FromRoute] Guid userId)
    {
        throw new NotImplementedException();
    }
}
