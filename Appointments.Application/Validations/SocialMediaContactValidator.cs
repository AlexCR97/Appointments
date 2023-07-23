using Appointments.Domain.Entities;
using FluentValidation;

namespace Appointments.Application.Validations;

internal sealed class SocialMediaContactValidator : AbstractValidator<SocialMediaContact>
{
    public SocialMediaContactValidator()
    {
        RuleFor(x => x.Contact)
            .NotEmpty()
            .MaxNameLength();

        When(x => x.Type != SocialMediaType.Other, () =>
        {
            RuleFor(x => x.OtherType)
                .Empty();
        });

        When(x => !string.IsNullOrWhiteSpace(x.OtherType), () =>
        {
            RuleFor(x => x.Type)
                .Must(type => type == SocialMediaType.Other);
        });
    }
}
