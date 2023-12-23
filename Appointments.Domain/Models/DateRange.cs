using FluentValidation;

namespace Appointments.Domain.Models;

public readonly struct DateRange
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public bool Disabled { get; }

    public DateRange()
    {
        var defaultDateTime = new DateTime();

        StartDate = defaultDateTime;
        EndDate = defaultDateTime.AddDays(1);
        Disabled = false;

        new DateRangeValidator().ValidateAndThrow(this);
    }

    public DateRange(DateTime startDate, DateTime endDate, bool disabled)
    {
        StartDate = startDate;
        EndDate = endDate;
        Disabled = disabled;

        new DateRangeValidator().ValidateAndThrow(this);
    }

    public override string ToString()
    {
        return $"{StartDate.ToShortDateString()} - {EndDate.ToShortDateString()}";
    }

    public static DateRange NineToFive()
    {
        return new DateRange(
            new DateTime().AddHours(9),
            new DateTime().AddHours(17),
            false);
    }
}

public sealed class DateRangeValidator : AbstractValidator<DateRange>
{
    public DateRangeValidator()
    {
        RuleFor(x => x.StartDate)
            .Must((dateRange, startDate) => startDate < dateRange.EndDate)
            .WithMessage("Must be less than EndDate");

        RuleFor(x => x.EndDate)
            .Must((dateRange, endDate) => endDate > dateRange.StartDate)
            .WithMessage("Must be greater than StartDate");
    }
}
