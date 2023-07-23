using Appointments.Api.Exceptions;
using Appointments.Api.Extensions.Files;
using Appointments.Application.Requests.Users;
using Appointments.Application.Requests.Users.Login;
using Appointments.Application.Requests.Users.SignUp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-up/email", Name = nameof(SignUpWithEmail))]
    public async Task<IActionResult> SignUpWithEmail([FromBody] SignUpWithEmailRequest request)
    {
        var userId = await _mediator.Send(request);

        return CreatedAtRoute(
            nameof(GetUserProfile),
            new { userId },
            new { userId });
    }

    [HttpPost("login/email", Name = nameof(LoginWithEmail))]
    public async Task<IActionResult> LoginWithEmail([FromBody] LoginWithEmailRequest request)
    {
        var oAuthToken = await _mediator.Send(request);
        return Ok(oAuthToken);
    }

    [HttpGet("{userId}/profile", Name = nameof(GetUserProfile))]
    public async Task<IActionResult> GetUserProfile([FromRoute] Guid userId)
    {
        var userProfile = await _mediator.Send(new GetUserProfileRequest(userId));
        return Ok(userProfile);
    }

    [HttpPatch("{userId}/profile", Name = nameof(UpdateUserProfile))]
    public async Task<IActionResult> UpdateUserProfile(
        [FromRoute] Guid userId,
        [FromBody] UpdateProfileRequest request)
    {
        IdMismatchException.ThrowIfMismatch(userId, request.Id);
        await _mediator.Send(request);
        return Ok();
    }

    [HttpPost("{userId}/profile/image", Name = nameof(UploadUserProfileImage))]
    public async Task<IActionResult> UploadUserProfileImage(
        [FromRoute] Guid userId,
        [FromForm] IFormFile image)
    {
        var profileImagePath = await _mediator.Send(new UploadProfileImageRequest(
            userId,
            image.FileName,
            image.GetBytes()));

        return Ok(new { profileImage = profileImagePath });
    }
}
