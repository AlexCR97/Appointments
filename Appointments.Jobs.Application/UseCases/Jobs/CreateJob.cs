using Appointments.Common.Domain;
using Appointments.Jobs.Domain.Jobs;
using FluentValidation;
using MediatR;

namespace Appointments.Jobs.Application.UseCases.Jobs;

public sealed record CreateJobRequest(
    string CreatedBy,
    JobType Type,
    JobGroup Group,
    JobName Name,
    string? DisplayName)
    : IRequest<Guid>;

internal sealed class CreateJobRequestValidator : AbstractValidator<CreateJobRequest>
{
    public CreateJobRequestValidator()
    {
        RuleFor(x => x.CreatedBy)
            .NotEmpty();

        RuleFor(x => x.Type)
            .NotEqual(JobType.Unknown);
    }
}

internal sealed class CreateJobRequestHandler : IRequestHandler<CreateJobRequest, Guid>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IJobRepository _jobRepository;

    public CreateJobRequestHandler(IEventProcessor eventProcessor, IJobRepository jobRepository)
    {
        _eventProcessor = eventProcessor;
        _jobRepository = jobRepository;
    }

    public async Task<Guid> Handle(CreateJobRequest request, CancellationToken cancellationToken)
    {
        new CreateJobRequestValidator().ValidateAndThrow(request);
        
        var job = CreateJob(request);

        if (job.HasChanged)
        {
            await _jobRepository.CreateAsync(job);
            await _eventProcessor.ProcessAsync(job.Events);
        }

        return job.Id;
    }

    private static Job CreateJob(CreateJobRequest request)
    {
        if (request.Type == JobType.Unknown)
            throw new UnsupportedJobTypeException(request.Type);

        if (request.Type == JobType.LoginMethodConfirmationReminder)
        {
            return LoginMethodConfirmationReminderJob.Create(
                request.CreatedBy,
                request.Group,
                request.Name,
                request.DisplayName);
        }

        throw new UnsupportedJobTypeException(request.Type);
    }
}
