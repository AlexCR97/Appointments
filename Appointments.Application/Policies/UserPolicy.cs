namespace Appointments.Application.Policies;

public static class UserPolicy
{
    public const string PolicyName = "User";

    public static class Roles
    {
        public const string Owner = "User.Owner";
        public const string Admin = "User.Admin";
        public const string Writer = "User.Writer";
        public const string Reader = "User.Reader";
    }
}
