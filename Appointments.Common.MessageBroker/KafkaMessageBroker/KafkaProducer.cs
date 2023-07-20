using Appointments.Common.MessageBroker.Abstractions;
using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace Appointments.Common.MessageBroker.KafkaMessageBroker;

internal class KafkaProducer<TMessage> : IPublisher<TMessage>
{
    private readonly IKafkaProducerOptions<TMessage> _options;
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(IKafkaProducerOptions<TMessage> options, IProducer<string, string> producer)
    {
        _options = options;
        _producer = producer;
    }

    public async Task PublishAsync(TMessage message)
    {
        var messageType = message?.GetType()?.Name;
        var messageTypeEncoded = messageType is null
            ? null
            : Encoding.UTF8.GetBytes(messageType);

        await _producer.ProduceAsync(_options.Topic, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Timestamp = Timestamp.Default,
            Value = JsonSerializer.Serialize(message),
            Headers = new Headers
            {
                new Header("MessageType", messageTypeEncoded),
            },
        });
    }
}
