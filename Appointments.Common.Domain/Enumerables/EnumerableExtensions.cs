namespace Appointments.Common.Domain.Enumerables;

public static class EnumerableExtensions
{
    public static string JoinToString<T>(this IEnumerable<T> source, char separator)
    {
        return string.Join(separator, source);
    }
}
