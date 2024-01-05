namespace Appointments.Api.Tenant;

public static class TenantApiPolicy
{
    public static class Me
    {
        public const string Scope = "tenant/me";
    }

    public static class Tenants
    {
        public const string Scope = "tenant/tenants";
    }
}
