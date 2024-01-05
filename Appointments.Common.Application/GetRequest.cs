using FluentValidation;
using MediatR;

namespace Appointments.Common.Application;

public record GetRequest<T>(Guid Id) : IRequest<T>;

public sealed class GetRequestValidator<T> : AbstractValidator<GetRequest<T>>
{
    public GetRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
