﻿using Appointments.Jobs.Infrastructure.UseCases.Jobs;
using Appointments.Jobs.Infrastructure.UseCases.Triggers;

namespace Appointments.Jobs.Infrastructure.UseCases.Executions;

public sealed record ExecutionCancellationRequestedEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid ExecutionId,
    JobDto JobSnapshot,
    TriggerDto TriggerSnapshot);
