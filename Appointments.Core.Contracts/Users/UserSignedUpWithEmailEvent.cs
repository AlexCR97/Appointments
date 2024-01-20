namespace Appointments.Core.Contracts.Users;

public sealed record UserSignedUpWithEmailEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId,
    string Email,
    string FullName,
    string ConfirmationCode);
