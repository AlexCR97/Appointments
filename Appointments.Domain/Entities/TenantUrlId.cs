using Appointments.Common.Utils.Security;
using FluentValidation;

namespace Appointments.Core.Domain.Entities;

public readonly struct TenantUrlId
{
    public const int MinLength = 1;
    public const int DefaultLength = 8;
    public const int MaxLength = 32;

    public string Value { get; }

    public TenantUrlId()
    {
        Value = string.Empty;
        new TenantUrlIdValidator().ValidateAndThrow(this);
    }

    public TenantUrlId(string value)
    {
        Value = value;
        new TenantUrlIdValidator().ValidateAndThrow(this);
    }

    public override string ToString()
    {
        return Value;
    }

    public static TenantUrlId Random()
    {
        var randomKey = KeyGenerator.Random(DefaultLength);
        return new TenantUrlId(randomKey);
    }
}

public sealed class TenantUrlIdValidator : AbstractValidator<TenantUrlId>
{
    public TenantUrlIdValidator()
    {
        RuleFor(x => x.Value)
            .MinimumLength(TenantUrlId.MinLength);

        RuleFor(x => x.Value)
            .MaximumLength(TenantUrlId.MaxLength);
    }
}
