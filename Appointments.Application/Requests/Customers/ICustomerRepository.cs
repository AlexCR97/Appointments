using Appointments.Common.Application;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Application.Requests.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
}
