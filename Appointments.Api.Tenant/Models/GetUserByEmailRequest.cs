using Appointments.Common.Domain.Models;

namespace Appointments.Api.Tenant.Models;

public sealed record GetUserByEmailRequest(string Email)
{
    internal Appointments.Core.Application.Requests.Users.GetUserByEmailRequest ToApplicationRequest()
    {
        return new Appointments.Core.Application.Requests.Users.GetUserByEmailRequest(
            new Email(Email));
    }
}
