using Appointments.Common.Domain;
using Appointments.Jobs.Application.UseCases.Jobs;
using Appointments.Jobs.Application.UseCases.Triggers;
using Appointments.Jobs.Domain.Executions;
using Appointments.Jobs.Domain.Triggers;
using FluentValidation;
using MediatR;

namespace Appointments.Jobs.Application.UseCases.Executions;

public sealed record EnqueueExecutionRequest(
    string CreatedBy,
    Guid JobId,
    TriggerType TriggerType)
    : IRequest<ExecutionQueuedResponse>;

public sealed record ExecutionQueuedResponse(
    Guid ExecutionId);

internal sealed class EnqueueExecutionRequestValidator : AbstractValidator<EnqueueExecutionRequest>
{
    public EnqueueExecutionRequestValidator()
    {
        RuleFor(x => x.CreatedBy)
            .NotEmpty();

        RuleFor(x => x.JobId)
            .NotEmpty();
    }
}

internal sealed class EnqueueExecutionRequestHandler : IRequestHandler<EnqueueExecutionRequest, ExecutionQueuedResponse>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IExecutionRepository _executionRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ITriggerRepository _triggerRepository;

    public EnqueueExecutionRequestHandler(IEventProcessor eventProcessor, IExecutionRepository executionRepository, IJobRepository jobRepository, ITriggerRepository triggerRepository)
    {
        _eventProcessor = eventProcessor;
        _executionRepository = executionRepository;
        _jobRepository = jobRepository;
        _triggerRepository = triggerRepository;
    }

    public async Task<ExecutionQueuedResponse> Handle(EnqueueExecutionRequest request, CancellationToken cancellationToken)
    {
        new EnqueueExecutionRequestValidator().ValidateAndThrow(request);

        var job = await _jobRepository.GetAsync(request.JobId);
        var trigger = await _triggerRepository.GetByTypeAsync(request.TriggerType);

        var execution = Execution.Enqueue(
            request.CreatedBy,
            job,
            trigger);

        if (execution.HasChanged)
        {
            await _executionRepository.CreateAsync(execution);
            await _eventProcessor.ProcessAsync(execution.Events);
        }

        return new ExecutionQueuedResponse(execution.Id);
    }
}
