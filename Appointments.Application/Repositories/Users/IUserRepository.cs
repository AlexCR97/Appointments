using Appointments.Application.Repositories.Abstractions;
using Appointments.Domain.Entities;

namespace Appointments.Application.Repositories.Users;

public interface IUserRepository : IRepository<User>
{
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetByEmailOrDefaultAsync(string email);
}
