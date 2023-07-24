namespace Appointments.Application.Policies;

public static class ServicePolicy
{
    public const string PolicyName = "Service";

    public static class Roles
    {
        public const string Owner = "Service.Owner";
        public const string Admin = "Service.Admin";
        public const string Writer = "Service.Writer";
        public const string Reader = "Service.Reader";
    }
}
