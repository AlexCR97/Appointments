using Appointments.Domain.Events.Abstractions;
using System.Text.Json;

namespace Appointments.Domain.Events;

public class Event : IEvent
{
    public override string ToString()
        => JsonSerializer.Serialize(this);
}
