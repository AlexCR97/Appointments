using Appointments.Common.Domain.Enums;
using Appointments.Jobs.Domain.Jobs;

namespace Appointments.Api.Management.Models;

public record CreateJobRequest(
    string Type,
    string Group,
    string Name,
    string? DisplayName);

internal static class CreateJobRequestExtensions
{
    public static Jobs.Application.UseCases.Jobs.CreateJobRequest ToApplicationRequest(
        this CreateJobRequest request,
        string createdBy)
    {
        return new Jobs.Application.UseCases.Jobs.CreateJobRequest(
            createdBy,
            request.Type.ToEnum<JobType>(),
            new JobGroup(request.Group),
            new JobName(request.Name),
            request.DisplayName);
    }
}
