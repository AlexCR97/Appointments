using Appointments.Common.Domain.Models;

namespace Appointments.Api.Connect.Models;

public sealed record LoginWithEmailRequest(
    string Email,
    string Password,
    string? Scope,
    Guid? TenantId)
{
    internal Appointments.Core.Application.Requests.Users.LoginWithEmailRequest ToApplicationRequest()
    {
        return new Appointments.Core.Application.Requests.Users.LoginWithEmailRequest(
            new Email(Email),
            Password,
            Scope,
            TenantId);
    }
}
