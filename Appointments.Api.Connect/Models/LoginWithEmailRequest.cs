using Appointments.Common.Domain.Models;

namespace Appointments.Api.Connect.Models;

public sealed record LoginWithEmailRequest(
    string Email,
    string Password,
    string? Scope,
    Guid? TenantId)
{
    public Core.Application.Requests.Users.LoginWithEmailRequest ToApplicationRequest()
    {
        return new Core.Application.Requests.Users.LoginWithEmailRequest(
            new Email(Email),
            Password,
            Scope,
            TenantId);
    }
}
