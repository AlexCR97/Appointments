using Appointments.Application.Services.Events;
using Appointments.Domain.Events.Abstractions;
using Microsoft.Extensions.Logging;

namespace Appointments.Infrastructure.Services.Events;

internal class EventProcessor : IEventProcessor
{
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(ILogger<EventProcessor> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(IEnumerable<IEvent> events)
    {
        _logger.LogWarning("Not implemented");
        return Task.CompletedTask;
    }
}
