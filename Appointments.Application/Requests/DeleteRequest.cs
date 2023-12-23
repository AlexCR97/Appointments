using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests;

public record DeleteRequest(
    string DeletedBy,
    Guid Id)
    : IRequest;

internal sealed class DeleteRequestValidator : AbstractValidator<DeleteRequest>
{
    public DeleteRequestValidator()
    {
        RuleFor(x => x.DeletedBy)
            .NotEmpty();

        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
