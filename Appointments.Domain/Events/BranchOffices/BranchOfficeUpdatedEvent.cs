using Appointments.Domain.Entities;
using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.BranchOffices;

internal sealed record BranchOfficeUpdatedEvent(
    DateTime UpdatedAt,
    string? UpdatedBy,
    string Name,
    Location Location,
    string Address,
    List<SocialMediaContact> SocialMediaContacts) : IEvent;
