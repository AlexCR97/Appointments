namespace Appointments.Application.Policies;

public static class BranchOfficePolicy
{
    public const string PolicyName = "BranchOffice";

    public static class Roles
    {
        public const string Owner = "BranchOffice.Owner";
        public const string Admin = "BranchOffice.Admin";
        public const string Writer = "BranchOffice.Writer";
        public const string Reader = "BranchOffice.Reader";
    }
}
