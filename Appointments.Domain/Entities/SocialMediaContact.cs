using Appointments.Common.Domain.Validations;
using FluentValidation;

namespace Appointments.Core.Domain.Entities;

public sealed record SocialMediaContact(
    SocialMediaType Type,
    string? OtherType,
    string Contact);

public sealed class SocialMediaContactValidator : AbstractValidator<SocialMediaContact>
{
    public SocialMediaContactValidator()
    {
        RuleFor(x => x.Contact)
            .NotEmpty()
            .MaxNameLength();

        When(x => x.Type != SocialMediaType.Other, () => RuleFor(x => x.OtherType)
            .Empty());

        When(x => !string.IsNullOrWhiteSpace(x.OtherType), () => RuleFor(x => x.Type)
            .Must(type => type == SocialMediaType.Other));
    }
}
