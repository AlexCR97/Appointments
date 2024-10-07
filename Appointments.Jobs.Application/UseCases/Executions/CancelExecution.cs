using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Jobs.Application.UseCases.Executions;

public sealed record CancelExecutionRequest(
    string UpdatedBy,
    Guid ExecutionId)
    : IRequest;

internal sealed class CancelExecutionRequestValidator : AbstractValidator<CancelExecutionRequest>
{
    public CancelExecutionRequestValidator()
    {
        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        RuleFor(x => x.ExecutionId)
            .NotEmpty();
    }
}

internal sealed class CancelExecutionRequestHandler : IRequestHandler<CancelExecutionRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IExecutionRepository _executionRepository;

    public CancelExecutionRequestHandler(IEventProcessor eventProcessor, IExecutionRepository executionRepository)
    {
        _eventProcessor = eventProcessor;
        _executionRepository = executionRepository;
    }

    public async Task Handle(CancelExecutionRequest request, CancellationToken cancellationToken)
    {
        new CancelExecutionRequestValidator().ValidateAndThrow(request);

        var execution = await _executionRepository.GetAsync(request.ExecutionId);

        execution.RequestCancellation(request.UpdatedBy);

        if (execution.HasChanged)
        {
            await _executionRepository.UpdateAsync(execution);
            await _eventProcessor.ProcessAsync(execution.Events);
        }
    }
}
