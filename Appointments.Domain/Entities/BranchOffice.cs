namespace Appointments.Domain.Entities;

public class BranchOffice : Entity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public Location Location { get; set; }
    public string Address { get; set; }
    public List<SocialMediaContact>? SocialMediaContacts { get; set; }
    public WeeklySchedule? WeeklySchedule { get; set; }
}
