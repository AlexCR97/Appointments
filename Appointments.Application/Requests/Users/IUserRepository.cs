using Appointments.Domain.Entities;
using Appointments.Domain.Models;

namespace Appointments.Application.Requests.Users;

public interface IUserRepository : IRepository<User>
{
    Task<bool> ExistsByEmailAsync(Email email);
    Task<User?> GetByEmailOrDefaultAsync(Email email);
}
