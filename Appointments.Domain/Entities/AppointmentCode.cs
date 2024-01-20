using Appointments.Common.Utils.Security;
using FluentValidation;

namespace Appointments.Core.Domain.Entities;

public readonly struct AppointmentCode
{
    public const int Length = 8;

    public string Value { get; }

    public AppointmentCode()
    {
        Value = string.Empty;
    }

    public AppointmentCode(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    public static AppointmentCode Random()
    {
        var randomKey = KeyGenerator.Random(Length);
        return new AppointmentCode(randomKey);
    }
}

public sealed class AppointmentCodeValidator : AbstractValidator<AppointmentCode>
{
    public AppointmentCodeValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .Length(AppointmentCode.Length);
    }
}
