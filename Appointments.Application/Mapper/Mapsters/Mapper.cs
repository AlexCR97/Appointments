using Appointments.Application.Mapper.Abstractions;

namespace Appointments.Application.Mapper.Mapsters;

internal class Mapper : IMapper
{
    private readonly MapsterMapper.IMapper _mapper;

    public Mapper(MapsterMapper.IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TDestination>(object source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        var mapped = _mapper.Map<TDestination>(source);
        return mapped;
    }
}
