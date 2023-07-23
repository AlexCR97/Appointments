using Appointments.Application.Requests.Tenants;
using Appointments.Domain.Entities;
using FluentValidation;

namespace Appointments.Application.Validations.Tenants;

internal sealed class UpdateTenantRequestValidator : AbstractValidator<UpdateTenantRequest>
{
    public UpdateTenantRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaxNameLength();

        When(x => !string.IsNullOrWhiteSpace(x.Slogan), () =>
        {
            RuleFor(x => x.Slogan!)
                .NotEmpty()
                .MaxDescriptionLength();
        });

        RuleFor(x => x.UrlId)
            .NotEmpty()
            .Length(Tenant.UrlIdLength);

        RuleForEach(x => x.SocialMediaContacts)
            .SetValidator(new SocialMediaContactValidator());
    }
}
