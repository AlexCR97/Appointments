using Appointments.Common.MessageBroker.Abstractions;
using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace Appointments.Common.MessageBroker.KafkaMessageBroker;

internal class KafkaProducer<TTopic> : IPublisher<TTopic>
    where TTopic : IKafkaTopic
{
    private readonly IProducer<string, string> _producer;
    private readonly TTopic _topic;

    public KafkaProducer(IProducer<string, string> producer, TTopic topic)
    {
        _producer = producer;
        _topic = topic;
    }

    public async Task PublishAsync<TMessage>(TMessage message)
    {
        await _producer.ProduceAsync(_topic.Topic, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Timestamp = Timestamp.Default,
            Value = JsonSerializer.Serialize(message),
            Headers = new Headers
            {
                CreateMessageTypeHeader(message),
            },
        });
    }

    private static Header CreateMessageTypeHeader<TMessage>(TMessage message)
    {
        var messageType = message?.GetType()?.Name;
        
        var messageTypeEncoded = messageType is null
            ? null
            : Encoding.UTF8.GetBytes(messageType);

        return new Header("MessageType", messageTypeEncoded);
    }
}
