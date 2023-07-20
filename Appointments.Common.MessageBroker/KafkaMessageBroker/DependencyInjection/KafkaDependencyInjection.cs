using Appointments.Common.MessageBroker.Abstractions;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Common.MessageBroker.KafkaMessageBroker.DependencyInjection;

public static class KafkaDependencyInjection
{
    public static IServiceCollection AddKafkaMessageBroker(this IServiceCollection services, IKafkaMessageBrokerOptions options)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = options.BootstrapServers,
            AllowAutoCreateTopics = true,
        };

        var producer = new ProducerBuilder<string, string>(producerConfig).Build();

        services.AddSingleton<IProducer<string, string>>(producer);
        
        return services;
    }

    public static IServiceCollection AddKafkaProducer<TMessage>(this IServiceCollection services, IKafkaProducerOptions<TMessage> options)
    {
        return services
            .AddSingleton<IKafkaProducerOptions<TMessage>>(options)
            .AddTransient<IPublisher<TMessage>, KafkaProducer<TMessage>>();
    }
}
