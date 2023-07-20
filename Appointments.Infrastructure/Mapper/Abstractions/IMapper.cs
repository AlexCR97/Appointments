namespace Appointments.Infrastructure.Mapper.Abstractions;

public interface IMapper
{
    public TDestination Map<TDestination>(object source);
}
