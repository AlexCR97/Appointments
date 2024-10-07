using Appointments.Common.Application;
using Appointments.Jobs.Domain.Jobs;

namespace Appointments.Jobs.Application.UseCases.Jobs;

public interface IJobRepository : IRepository<Job>
{
}
