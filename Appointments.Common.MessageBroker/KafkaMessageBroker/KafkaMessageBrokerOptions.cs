namespace Appointments.Common.MessageBroker.KafkaMessageBroker;

public interface IKafkaMessageBrokerOptions
{
    public string BootstrapServers { get; }
}

public class KafkaMessageBrokerOptions : IKafkaMessageBrokerOptions
{
    public string BootstrapServers { get; }

    public KafkaMessageBrokerOptions(string bootstrapServers)
    {
        BootstrapServers = bootstrapServers;
    }
}
