using FluentValidation;

namespace Appointments.Domain.Models;

public readonly struct Address
{
    public Coordinates Coordinates { get; }
    public string Description { get; }

    public Address()
    {
        Coordinates = Coordinates.Default();
        Description = string.Empty;
        new AddressValidator().ValidateAndThrow(this);
    }

    public Address(Coordinates coordinates, string description)
    {
        Coordinates = coordinates;
        Description = description;
        new AddressValidator().ValidateAndThrow(this);
    }

    public override string ToString()
    {
        return $"{Description} ({Coordinates})";
    }

    public static Address Default()
    {
        return new Address();
    }
}

public sealed class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Coordinates)
            .SetValidator(new CoordinatesValidator());

        RuleFor(x => x.Description)
            .NotNull();
    }
}
