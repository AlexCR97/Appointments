namespace Appointments.Domain.Models;

public record UserProfile(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? ProfileImage,
    IReadOnlyDictionary<string, object?>? Extensions);
