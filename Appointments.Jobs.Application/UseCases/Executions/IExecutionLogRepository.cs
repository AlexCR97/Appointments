using Appointments.Common.Application;
using Appointments.Common.Domain.Models;
using Appointments.Jobs.Domain.Executions;

namespace Appointments.Jobs.Application.UseCases.Executions;

public interface IExecutionLogRepository : IRepository<ExecutionLog>
{
    Task<PagedResult<ExecutionLog>> FindAsync(int pageIndex, int pageSize, Guid? executionId = null);
}
