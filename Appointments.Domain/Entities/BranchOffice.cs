using Appointments.Domain.Events.BranchOffices;

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
        Guid id,
        DateTime createdAt,
        string? createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        DateTime? deletedAt,
        string? deletedBy,
        List<string> tags,
        Dictionary<string, string?> extensions,

        Guid tenantId,
        string name,
        Location location,
        string address,
        List<SocialMediaContact> socialMediaContacts,
        WeeklySchedule weeklySchedule)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy,
        deletedAt,
        deletedBy,
        tags,
        extensions)
    {
        TenantId = tenantId;
        Name = name;
        Location = location;
        Address = address;
        SocialMediaContacts = socialMediaContacts;
        WeeklySchedule = weeklySchedule;
    }

    public static BranchOffice CreateMinimal(
        string? createdBy,
        Guid tenantId)
    {
        var branchOffice = new BranchOffice(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            null,
            null,
            new List<string>(),
            new Dictionary<string, string?>(),

            tenantId,
            string.Empty,
            Location.Empty,
            string.Empty,
            new(),
            WeeklySchedule.NineToFive);

        branchOffice.AddEvent(new MinimalBranchOfficeCreatedEvent(
            branchOffice.Id,
            branchOffice.CreatedAt,
            createdBy,
            tenantId,
            Location.Empty,
            WeeklySchedule.NineToFive));

        return branchOffice;
    }
}
