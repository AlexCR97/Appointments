namespace Appointments.Common.MessageBroker.KafkaMessageBroker;

public interface IKafkaOptions
{
    public string BootstrapServers { get; }
}

public class KafkaOptions : IKafkaOptions
{
    public string BootstrapServers { get; }

    public KafkaOptions(string bootstrapServers)
    {
        BootstrapServers = bootstrapServers;
    }
}
