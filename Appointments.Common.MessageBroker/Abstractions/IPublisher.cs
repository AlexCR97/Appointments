namespace Appointments.Common.MessageBroker.Abstractions;

public interface IPublisher<TMessage>
{
    Task PublishAsync(TMessage message);
}
