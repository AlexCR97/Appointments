using Appointments.Infrastructure.Mapper.Abstractions;
using Mapster;

namespace Appointments.Infrastructure.Mapper.Mapsters;

internal class MapsterMapper : IMapper
{
    public TDestination Map<TDestination>(object source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        return source.Adapt<TDestination>();
    }
}
