namespace Appointments.Application.Policies;

public static class TenantPolicy
{
    public const string PolicyName = "Tenant";

    public static class Roles
    {
        public const string Owner = "Tenant.Owner";
        public const string Admin = "Tenant.Admin";
        public const string Writer = "Tenant.Writer";
        public const string Reader = "Tenant.Reader";
    }
}
