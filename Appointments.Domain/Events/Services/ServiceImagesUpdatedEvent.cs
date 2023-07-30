using Appointments.Domain.Entities;
using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Services;

public sealed record ServiceImagesUpdatedEvent(
    Guid Id,
    DateTime UpdatedAt,
    string? UpdatedBy,
    List<IndexedImage> Images) : IEvent;
