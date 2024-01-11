using Appointments.Api.Connect.Models;
using Appointments.Common.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Connect.Controllers;

[ApiController]
[Route("api/connect")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class ConnectController : ControllerBase
{
    private readonly ISender _sender;

    public ConnectController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("sign-up/email", Name = nameof(SignUpWithEmail))]
    public async Task<Core.Application.Requests.Users.UserSignedUpResult> SignUpWithEmail([FromBody] SignUpWithEmailRequest request)
    {
        return await _sender.Send(request.ToApplicationRequest());
    }

    [HttpPost("login/email", Name = nameof(LoginWithEmail))]
    public async Task<OAuthToken> LoginWithEmail([FromBody] LoginWithEmailRequest request)
    {
        return await _sender.Send(request.ToApplicationRequest());
    }
}
