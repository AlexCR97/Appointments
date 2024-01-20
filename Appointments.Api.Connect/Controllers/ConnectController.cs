using Appointments.Api.Connect.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Connect.Controllers;

[ApiController]
[Route("api/connect")]
[ApiVersion("1.0")]
[Produces("application/json")]
[AllowAnonymous]
public class ConnectController : ControllerBase
{
    private readonly ISender _sender;

    public ConnectController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("sign-up/email", Name = nameof(SignUpWithEmail))]
    public async Task<UserSignedUpResponse> SignUpWithEmail([FromBody] SignUpWithEmailRequest request)
    {
        var result = await _sender.Send(request.ToApplicationRequest());
        return UserSignedUpResponse.From(result);
    }

    [HttpGet("confirm/email")]
    public async Task<EmailConfirmationResponse> ConfirmEmail([FromQuery] string code)
    {
        var result = await _sender.Send(new Appointments.Core.Application.Requests.Users.ConfirmEmailRequest(code));
        return new EmailConfirmationResponse(result.ToString());
    }

    [HttpPost("login/email", Name = nameof(LoginWithEmail))]
    public async Task<OAuthTokenResponse> LoginWithEmail([FromBody] LoginWithEmailRequest request)
    {
        var token = await _sender.Send(request.ToApplicationRequest());
        return OAuthTokenResponse.From(token);
    }
}
