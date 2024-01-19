using FluentValidation;

namespace Appointments.Common.Domain.Validations;

public static class ValidationsExtensions
{
    private const int _maxNameLength = 30;

    public static IRuleBuilder<T, string> MaxNameLength<T>(this IRuleBuilder<T, string> builder)
    {
        return builder
            .MaximumLength(_maxNameLength);
    }

    private const int _maxDescriptionLength = 1000;

    public static IRuleBuilder<T, string> MaxDescriptionLength<T>(this IRuleBuilder<T, string> builder)
    {
        return builder
            .MaximumLength(_maxDescriptionLength);
    }

    private const int _minPasswordLength = 6;
    private const int _maxPasswordLength = 256;

    /// <summary>
    /// Copied and adapted from https://stackoverflow.com/questions/63864594/how-can-i-create-strong-passwords-with-fluentvalidation
    /// </summary>
    public static IRuleBuilderOptions<TSource, string> Password<TSource>(this IRuleBuilder<TSource, string> ruleBuilder)
    {
        return ruleBuilder
            .MinimumLength(_minPasswordLength).WithMessage($"Your password length must be at least {_minPasswordLength}.")
            .MaximumLength(_maxPasswordLength).WithMessage($"Your password length must not exceed {_maxPasswordLength}.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least 1 uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least 1 lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least 1 number.");
    }
}
