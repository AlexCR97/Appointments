using FluentValidation;
using MediatR;

namespace Appointments.Common.Application;

public record DeleteRequest(
    string DeletedBy,
    Guid Id)
    : IRequest;

public sealed class DeleteRequestValidator : AbstractValidator<DeleteRequest>
{
    public DeleteRequestValidator()
    {
        RuleFor(x => x.DeletedBy)
            .NotEmpty();

        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
