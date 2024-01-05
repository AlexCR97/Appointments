using Appointments.Common.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Common.Application;

public abstract record FindRequest<T>(
    int PageIndex,
    int PageSize,
    string? Sort,
    string? Filter)
    : IRequest<PagedResult<T>>;

public sealed class FindRequestValidator<T> : AbstractValidator<FindRequest<T>>
{
    private const int _minPageSize = 1;
    private const int _maxPageSize = 100;

    public FindRequestValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(_minPageSize)
            .LessThanOrEqualTo(_maxPageSize);
    }
}
