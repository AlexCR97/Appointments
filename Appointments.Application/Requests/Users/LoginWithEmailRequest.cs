using Appointments.Common.Domain.Models;
using Appointments.Core.Application.Jwt;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Users;

public sealed record LoginWithEmailRequest(
    Email Email,
    string Password,
    string? Scope,
    Guid? TenantId)
    : IRequest<OAuthToken>;

internal sealed class LoginWithEmailRequestValidator : AbstractValidator<LoginWithEmailRequest>
{
    public LoginWithEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .SetValidator(new EmailValidator());

        RuleFor(x => x.Password)
            .NotEmpty();

        When(x => x.Scope is not null, () => RuleFor(x => x.Scope)
            .NotEmpty());

        When(x => x.TenantId is not null, () => RuleFor(x => x.TenantId)
            .NotEmpty());
    }
}

internal sealed class LoginWithEmailRequestHandler : IRequestHandler<LoginWithEmailRequest, OAuthToken>
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
        new LoginWithEmailRequestValidator().ValidateAndThrow(request);

        var user = await _userService.LoginWithEmailAsync(request.Email, request.Password);

        var userEmail = user
            .GetLocalLogin()
            .GetEmail();

        var userTenant = request.TenantId is null
            ? null
            : user.GetTenantOrDefault(request.TenantId.Value)
                ?? throw new InvalidUserTenantException(request.TenantId.Value);

        return new OAuthToken(
            GenerateAccessToken(userEmail, request.Scope, userTenant),
            "Bearer",
            _jwtOptions.ExpiresInMinutes,
            request.Scope,
            GenerateIdToken(userEmail, user),
            GenerateRefreshToken(userEmail, request.Scope, userTenant));
    }

    private string GenerateIdToken(
        Email email,
        User user)
    {
        var userClaims = new IdTokenClaims(
            user.Id,
            email.Value,
            user.FirstName,
            user.LastName,
            user.ProfileImage);

        return _jwtService.GenerateJwt(userClaims.ToClaims());
    }

    private string GenerateAccessToken(
        Email email,
        string? scope,
        UserTenant? userTenant)
    {
        var userClaims = new AccessTokenClaims(
            email.Value,
            scope,
            userTenant?.TenantId);

        return _jwtService.GenerateJwt(userClaims.ToClaims());
    }

    private string GenerateRefreshToken(
        Email email,
        string? scope,
        UserTenant? userTenant)
    {
        var refreshClaims = new RefreshTokenClaims(
            email.Value,
            scope,
            userTenant?.TenantId);

        return _jwtService.GenerateJwt(refreshClaims.ToClaims());
    }
}
