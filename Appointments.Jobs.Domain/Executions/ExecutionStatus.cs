using System.Collections.Immutable;

namespace Appointments.Jobs.Domain.Executions;

public enum ExecutionStatus
{
    /// <summary>
    /// The status of the execution is indeterminate.
    /// </summary>
    Unknown,

    /// <summary>
    /// The execution has been scheduled and is waiting to be started.
    /// </summary>
    Queued,

    /// <summary>
    /// The execution is currently in progress.
    /// </summary>
    Running,

    /// <summary>
    /// The execution was stopped before completion, either manually or due to external factors.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The execution has exceeded a predefined time limit without completion.
    /// </summary>
    TimedOut,

    /// <summary>
    /// The execution was forcefully terminated before completion.
    /// </summary>
    Aborted,

    /// <summary>
    /// The execution finished, but encountered one or more errors and did not complete successfully.
    /// </summary>
    Failed,

    /// <summary>
    /// The execution finished, but there might have been issues or warnings
    /// encountered during the process. While the job did not entirely fail,
    /// it also did not complete without problems.<br></br>
    /// </summary>
    Completed,

    /// <summary>
    /// The execution has finished successfully.
    /// </summary>
    Succeeded,
}

internal static class ExecutionStatusStateMachine
{
    private static readonly IReadOnlyDictionary<ExecutionStatus, IReadOnlySet<ExecutionStatus>> _transitions = new Dictionary<ExecutionStatus, IReadOnlySet<ExecutionStatus>>
    {
        [ExecutionStatus.Unknown] = ImmutableHashSet.CreateRange(new ExecutionStatus[]
        {
            ExecutionStatus.Queued,
            ExecutionStatus.Cancelled,
            ExecutionStatus.TimedOut,
            ExecutionStatus.Aborted,
        }),
        [ExecutionStatus.Queued] = ImmutableHashSet.CreateRange(new ExecutionStatus[]
        {
            ExecutionStatus.Running,
            ExecutionStatus.Cancelled,
            ExecutionStatus.Aborted,
        }),
        [ExecutionStatus.Running] = ImmutableHashSet.CreateRange(new ExecutionStatus[]
        {
            ExecutionStatus.Cancelled,
            ExecutionStatus.TimedOut,
            ExecutionStatus.Aborted,
            ExecutionStatus.Failed,
            ExecutionStatus.Completed,
            ExecutionStatus.Succeeded,
        }),
        [ExecutionStatus.Cancelled] = ImmutableHashSet<ExecutionStatus>.Empty,
        [ExecutionStatus.TimedOut] = ImmutableHashSet<ExecutionStatus>.Empty,
        [ExecutionStatus.Aborted] = ImmutableHashSet<ExecutionStatus>.Empty,
        [ExecutionStatus.Failed] = ImmutableHashSet<ExecutionStatus>.Empty,
        [ExecutionStatus.Completed] = ImmutableHashSet<ExecutionStatus>.Empty,
        [ExecutionStatus.Succeeded] = ImmutableHashSet<ExecutionStatus>.Empty,
    };

    public static bool CanTransitionTo(this ExecutionStatus from, ExecutionStatus to)
    {
        return _transitions[from].Contains(to);
    }
}
