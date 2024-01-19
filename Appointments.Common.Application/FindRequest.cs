using Appointments.Common.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Common.Application;

public static class FindRequest
{
    public static class PageSize
    {
        public const int Min = 1;
        public const int Default = 10;
        public const int Max = 100;
    }
}

public abstract record FindRequest<T>(
    int PageIndex,
    int PageSize,
    string? Sort,
    string? Filter)
    : IRequest<PagedResult<T>>;

public sealed class FindRequestValidator<T> : AbstractValidator<FindRequest<T>>
{
    public FindRequestValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(FindRequest.PageSize.Min)
            .LessThanOrEqualTo(FindRequest.PageSize.Max);
    }
}
