using Appointments.Common.Domain.Exceptions;

namespace Appointments.Core.Application.Requests.Users;

public class InvalidUserTenantException : DomainException
{
    public Guid TenantId { get; }

    public InvalidUserTenantException(Guid tenantId)
        : base(@$"Invalid tenant ""{tenantId}""")
    {
        TenantId = tenantId;
    }
}
