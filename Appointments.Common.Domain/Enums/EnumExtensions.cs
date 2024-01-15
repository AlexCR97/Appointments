namespace Appointments.Common.Domain.Enums;

public static class EnumExtensions
{
    public static T ToEnum<T>(this string value) where T : struct
    {
        return Enum.Parse<T>(value);
    }
}
