using FluentValidation;

namespace Appointments.Domain.Models;

public readonly struct Email
{
    public string Value { get; }

    public Email()
    {
        Value = "email@domain.com";
        new EmailValidator().ValidateAndThrow(this);
    }

    public Email(string value)
    {
        Value = value.Trim().ToLower();
        new EmailValidator().ValidateAndThrow(this);
    }

    public override string ToString()
    {
        return Value;
    }
}

public sealed class EmailValidator : AbstractValidator<Email>
{
    public EmailValidator()
    {
        RuleFor(x => x.Value)
            .EmailAddress();
    }
}
