namespace Appointments.Domain.Entities;

public class BranchOffice : Entity
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public Location Location { get; private set; }
    public string Address { get; private set; }
    public List<SocialMediaContact> SocialMediaContacts { get; private set; }
    public WeeklySchedule WeeklySchedule { get; private set; }

    public BranchOffice(
        Guid tenantId,
        string name,
        Location location,
        string address,
        List<SocialMediaContact> socialMediaContacts,
        WeeklySchedule weeklySchedule)
    {
        TenantId = tenantId;
        Name = name;
        Location = location;
        Address = address;
        SocialMediaContacts = socialMediaContacts;
        WeeklySchedule = weeklySchedule;
    }

    public static BranchOffice CreateSlim(
        string? createdBy,
        Guid tenantId)
    {
        var branchOffice = new BranchOffice(
            tenantId,
            string.Empty,
            Location.Empty,
            string.Empty,
            new(),
            WeeklySchedule.NineToFive)
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };

        branchOffice.AddEvent(new SlimBranchOfficeCreatedEvent(
            branchOffice.Id,
            branchOffice.CreatedAt,
            createdBy,
            tenantId,
            Location.Empty,
            WeeklySchedule.NineToFive));

        return branchOffice;
    }
}
