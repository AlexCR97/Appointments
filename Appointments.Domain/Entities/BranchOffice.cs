using Appointments.Domain.Models;

namespace Appointments.Domain.Entities;

public sealed class BranchOffice : Entity
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public Address Address { get; private set; }

    private readonly List<SocialMediaContact> _contacts = new();
    public IReadOnlyList<SocialMediaContact> Contacts
    {
        get
        {
            return _contacts;
        }
        private set
        {
            _contacts.Clear();
            _contacts.AddRange(value);
        }
    }

    public WeeklySchedule? Schedule { get; private set; }

    public BranchOffice(
        Guid id,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        Guid tenantId,
        string name,
        Address address,
        IReadOnlyList<SocialMediaContact> socialMediaContacts,
        WeeklySchedule? weeklySchedule)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy)
    {
        TenantId = tenantId;
        Name = name;
        Address = address;
        Contacts = socialMediaContacts;
        Schedule = weeklySchedule;
    }

    public void Update(
        string updatedBy,
        string name,
        Address address,
        IReadOnlyList<SocialMediaContact> socialMediaContacts)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        Name = name;
        Address = address;
        Contacts = socialMediaContacts;

        // TODO Add event
    }

    public static BranchOffice Create(
        string createdBy,
        Guid tenantId,
        string name,
        Address address,
        IReadOnlyList<SocialMediaContact> socialMediaContacts,
        WeeklySchedule? weeklySchedule)
    {
        var branchOffice = new BranchOffice(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            tenantId,
            name,
            address,
            socialMediaContacts,
            weeklySchedule);

        // TODO Add event

        return branchOffice;
    }
}
