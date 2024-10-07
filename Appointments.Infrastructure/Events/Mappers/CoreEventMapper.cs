using Appointments.Core.Contracts.Users;

namespace Appointments.Infrastructure.Events.Mappers;

internal sealed class CoreEventMapper : IEventMapper
{
    public object ToIntegrationEvent(object domainEvent)
    {
        if (domainEvent is Core.Application.Requests.Users.UserSignedUpWithEmailEvent userSignedUpWithEmailEvent)
        {
            return new UserSignedUpWithEmailEvent(
                userSignedUpWithEmailEvent.Id,
                userSignedUpWithEmailEvent.OccurredAt,
                userSignedUpWithEmailEvent.UserId,
                userSignedUpWithEmailEvent.Email.Value,
                userSignedUpWithEmailEvent.FullName,
                userSignedUpWithEmailEvent.ConfirmationCode);
        }

        throw new IntegrationEventException(domainEvent);
    }
}
