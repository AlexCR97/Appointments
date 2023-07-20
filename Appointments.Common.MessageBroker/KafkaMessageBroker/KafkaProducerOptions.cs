namespace Appointments.Common.MessageBroker.KafkaMessageBroker;

public interface IKafkaProducerOptions<TMessage>
{
    public string Topic { get; }
}

public class KafkaProducerOptions<TMessage> : IKafkaProducerOptions<TMessage>
{
    public string Topic { get; }

    public KafkaProducerOptions(string topic)
    {
        Topic = topic;
    }
}
