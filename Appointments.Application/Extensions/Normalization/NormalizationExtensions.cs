namespace Appointments.Application.Extensions.Normalization;

internal static class NormalizationExtensions
{
    public static string NormalizeForComparison(this string str)
    {
        return str
            .Trim()
            .ToLower();
    }
}
