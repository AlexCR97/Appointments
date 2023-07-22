using Appointments.Application.Requests;
using FluentValidation;

namespace Appointments.Application.Validations;

internal class GetPagedRequestValidator<T> : AbstractValidator<GetPagedRequest<T>>
{
    public GetPagedRequestValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(GetPagedRequest<T>.MaxPageSize);
    }
}
