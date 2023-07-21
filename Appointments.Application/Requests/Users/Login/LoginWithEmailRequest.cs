using Appointments.Domain.Models.Auth;
using MediatR;

namespace Appointments.Application.Requests.Users.Login;

public record LoginWithEmailRequest(
    string Email,
    string Password) : IRequest<OAuthToken>;
