using Appointments.Common.Domain.Models;

namespace Appointments.Api.Tenant.Models;

public sealed record AddressModel(
    CoordinatesModel Coordinates,
    string Description)
{
    internal static AddressModel From(Address entity)
    {
        return new AddressModel(
            CoordinatesModel.From(entity.Coordinates),
            entity.Description);
    }

    internal Address ToModel()
    {
        return new Address(
            Coordinates.ToModel(),
            Description);
    }
}
