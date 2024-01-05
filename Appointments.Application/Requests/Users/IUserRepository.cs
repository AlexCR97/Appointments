using Appointments.Common.Application;
using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Application.Requests.Users;

public interface IUserRepository : IRepository<User>
{
    Task<bool> ExistsByEmailAsync(Email email);
    Task<User?> GetByEmailOrDefaultAsync(Email email);
}
