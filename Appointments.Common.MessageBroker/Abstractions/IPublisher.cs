namespace Appointments.Common.MessageBroker.Abstractions;

public interface IPublisher<TQueue>
    where TQueue : IQueue
{
    Task PublishAsync<TMessage>(TMessage message);
}
