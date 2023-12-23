using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests;

public record GetRequest<T>(Guid Id) : IRequest<T>;

internal sealed class GetRequestValidator<T> : AbstractValidator<GetRequest<T>>
{
    public GetRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
