using Appointments.Common.Domain.Models;

namespace Appointments.Infrastructure.Mongo.Documents;

internal sealed record AddressDocument(
    CoordinatesDocument Coordinates,
    string Description)
{
    internal static AddressDocument From(Address address)
    {
        return new AddressDocument(
            CoordinatesDocument.From(address.Coordinates),
            address.Description);
    }

    internal Address ToEntity()
    {
        return new Address(
            Coordinates.ToEntity(),
            Description);
    }
}
