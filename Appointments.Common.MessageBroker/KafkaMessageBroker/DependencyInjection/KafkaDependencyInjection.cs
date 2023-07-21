using Appointments.Common.MessageBroker.Abstractions;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Common.MessageBroker.KafkaMessageBroker.DependencyInjection;

public static class KafkaDependencyInjection
{
    public static IServiceCollection AddKafka(this IServiceCollection services, IKafkaOptions options)
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

    public static IServiceCollection AddKafkaProducer<TTopic>(this IServiceCollection services, TTopic topic)
        where TTopic : class, IKafkaTopic
    {
        return services
            .AddSingleton<TTopic>(topic)
            .AddTransient<IPublisher<TTopic>, KafkaProducer<TTopic>>();
    }
}
