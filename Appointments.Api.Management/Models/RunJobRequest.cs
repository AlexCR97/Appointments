using Appointments.Common.Domain.Enums;
using Appointments.Jobs.Domain.Triggers;

namespace Appointments.Api.Management.Models;

public record RunJobRequest(
    string TriggerType,
    double? Timeout);

internal static class RunJobRequestExtensions
{
    public static Jobs.Application.UseCases.Executions.EnqueueExecutionRequest ToApplicationRequest(
        this RunJobRequest request,
        string createdBy,
        Guid jobId)
    {
        return new Jobs.Application.UseCases.Executions.EnqueueExecutionRequest(
            createdBy,
            jobId,
            request.TriggerType.ToEnum<TriggerType>(),
            request.Timeout is null ? null : TimeSpan.FromMilliseconds(request.Timeout.Value));
    }
}
