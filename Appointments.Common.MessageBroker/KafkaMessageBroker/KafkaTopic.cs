using Appointments.Common.MessageBroker.Abstractions;

namespace Appointments.Common.MessageBroker.KafkaMessageBroker;

public interface IKafkaTopic : IQueue
{
    public string Topic { get; }
}
