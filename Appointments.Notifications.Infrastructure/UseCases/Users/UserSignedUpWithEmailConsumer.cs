using Appointments.Core.Contracts.Users;
using MassTransit;

namespace Appointments.Notifications.Infrastructure.UseCases.Users;

internal sealed class UserSignedUpWithEmailConsumer : IConsumer<UserSignedUpWithEmailEvent>
{
    public Task Consume(ConsumeContext<UserSignedUpWithEmailEvent> context)
    {
        // TODO Send email
        return Task.CompletedTask;
    }
}
