using Appointments.Common.Domain;
using Appointments.Common.Domain.Models;

namespace Appointments.Core.Application.Requests.Users;

public sealed record UserSignedUpWithEmailEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId,
    Email Email,
    string FullName,
    string ConfirmationCode)
    : IDomainEvent;
