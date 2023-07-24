using Appointments.Application.Policies;
using Appointments.Application.Services.Jwt;
using Appointments.Application.Services.Users;
using Appointments.Domain.Entities;
using Appointments.Domain.Models.Auth;
using MediatR;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Appointments.Application.Requests.Users.Login;

internal class LoginWithEmailRequestHandler : IRequestHandler<LoginWithEmailRequest, OAuthToken>
{
    private readonly IJwtOptions _jwtOptions;
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;

    public LoginWithEmailRequestHandler(IJwtOptions jwtOptions, IJwtService jwtService, IUserService userService)
    {
        _jwtOptions = jwtOptions;
        _jwtService = jwtService;
        _userService = userService;
    }

    public async Task<OAuthToken> Handle(LoginWithEmailRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.LoginAsync(request.Email, request.Password);
        return GenerateOAuthToken(user);
    }

    private OAuthToken GenerateOAuthToken(User user)
    {
        var accessToken = _jwtService.GenerateJwt(new List<Claim>
        {
            new Claim(
                "role",
                JsonConvert.SerializeObject(new string[]
                {
                    UserPolicy.Roles.Owner,
                    TenantPolicy.Roles.Owner,
                    ServicePolicy.Roles.Owner,
                    BranchOfficePolicy.Roles.Owner,
                }),
                JsonClaimValueTypes.JsonArray),
        });
        
        var idToken = _jwtService.GenerateJwt(new List<Claim>
        {
            new Claim("id", user.Id.ToString(), ClaimValueTypes.String),
            new Claim("email", user.Email, ClaimValueTypes.Email),
            new Claim("firstName", user.FirstName ?? string.Empty, ClaimValueTypes.String),
            new Claim("lastName", user.LastName ?? string.Empty, ClaimValueTypes.String),
        });

        var refreshToken = _jwtService.GenerateJwt(new List<Claim>
        {
            new Claim("email", user.Email, ClaimValueTypes.Email),
        });

        return new OAuthToken(
            accessToken,
            "Bearer",
            _jwtOptions.ExpiresInMinutes,
            null,
            idToken,
            refreshToken);
    }
}
