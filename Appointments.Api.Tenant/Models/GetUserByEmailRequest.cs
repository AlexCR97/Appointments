using Appointments.Common.Domain.Models;

namespace Appointments.Api.Tenant.Models;

public sealed record GetUserByEmailRequest(string Email)
{
    internal Core.Application.Requests.Users.GetUserByEmailRequest ToApplicationRequest()
    {
        return new Core.Application.Requests.Users.GetUserByEmailRequest(
            new Email(Email));
    }
}
