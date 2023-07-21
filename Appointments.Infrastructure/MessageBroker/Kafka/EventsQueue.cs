using Appointments.Common.MessageBroker.KafkaMessageBroker;

namespace Appointments.Infrastructure.MessageBroker.Kafka;

internal interface IEventsQueue : IKafkaTopic
{
}

internal class EventsQueue : IEventsQueue
{
    public string Topic { get; } = "Events";
}
