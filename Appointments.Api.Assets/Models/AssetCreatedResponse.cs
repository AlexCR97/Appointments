namespace Appointments.Api.Assets.Models;

public sealed record AssetCreatedResponse(
    Guid Id,
    string TransactionCode);
