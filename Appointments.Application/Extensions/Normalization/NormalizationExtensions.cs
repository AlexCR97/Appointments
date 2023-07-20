namespace Appointments.Application.Extensions.Normalization;

public static class NormalizationExtensions
{
    public static string NormalizeForComparison(this string str)
    {
        return str
            .Trim()
            .ToLower();
    }
}
