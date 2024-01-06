using Appointments.Api.Connect.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Connect.Controllers;

[ApiController]
[Route("api/connect")]
[ApiVersion("1.0")]
public class ConnectController : ControllerBase
{
    private readonly ISender _sender;

    public ConnectController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("sign-up/email")]
    public async Task<OkObjectResult> SignUpWithEmail([FromBody] SignUpWithEmailRequest request)
    {
        var result = await _sender.Send(request.ToApplicationRequest());
        return Ok(result);
    }

    [HttpPost("login/email")]
    public async Task<OkObjectResult> LoginWithEmail([FromBody] LoginWithEmailRequest request)
    {
        var result = await _sender.Send(request.ToApplicationRequest());
        return Ok(result);
    }
}
