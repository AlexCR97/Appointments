using Appointments.Domain.Entities;

namespace Appointments.Domain.Models.BranchOffices;

public class BranchOfficeModel : EntityModel
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = null!;
    public Location Location { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<SocialMediaContact> SocialMediaContacts { get; set; } = null!;
    public WeeklySchedule WeeklySchedule { get; set; } = null!;
}
