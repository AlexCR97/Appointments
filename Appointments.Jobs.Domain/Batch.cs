using Appointments.Common.Domain;
using Appointments.Jobs.Domain.Jobs;

namespace Appointments.Jobs.Domain;

/// <summary>
/// A sequence of Jobs to run.
/// </summary>
public class Batch : Entity
{
    public Batch(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        BatchExecutionMode executionMode, IReadOnlyList<BatchJob> jobs)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        ExecutionMode = executionMode;
        Jobs = jobs;
    }

    public BatchExecutionMode ExecutionMode { get; }

    public IReadOnlyList<BatchJob> Jobs { get; }
    private readonly List<BatchJob> _jobs;
}

public enum BatchExecutionMode
{
    Concurrent,
    Sequential,
}

public class BatchJob
{
    public BatchJob(Job job, Job? after)
    {
        Job = job;
        After = after;
    }

    public Job Job { get; }
    public Job? After { get; }
}
