using Appointments.Domain.Entities;
using PasswordGenerator;

namespace Appointments.Application.Services.Tenants;

internal static class TenantUrlIdGenerator
{
    public static string Random()
    {
        return new Password()
            .LengthRequired(Tenant.UrlIdLength)
            .IncludeUppercase()
            .IncludeLowercase()
            .IncludeNumeric()
            .Next();
    }
}
