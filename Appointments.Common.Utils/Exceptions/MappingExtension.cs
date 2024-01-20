namespace Appointments.Common.Utils.Exceptions;

public class MappingExtension<TSource, TDestination> : Exception
{
    public MappingExtension()
        : base($"Invalid mapping from type {typeof(TSource).Name} to type {typeof(TDestination).Name}")
    {
    }
}

public class MappingExtension : Exception
{
    public MappingExtension(Type source, Type destination)
        : base($"Invalid mapping from type {source.Name} to type {destination.Name}")
    {
    }
}
