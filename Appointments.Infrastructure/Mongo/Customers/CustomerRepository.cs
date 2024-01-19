using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Core.Application.Requests.Customers;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Infrastructure.Mongo.Customers;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly IMongoRepository<CustomerDocument> _repository;

    public CustomerRepository(IMongoRepository<CustomerDocument> repository)
    {
        _repository = repository;
    }

    public async Task<Customer> CreateAsync(Customer entity)
    {
        var document = CustomerDocument.From(entity);
        var createdDocument = await _repository.CreateAsync(document);
        return createdDocument.ToEntity();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PagedResult<Customer>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        var documents = await _repository.GetAsync(
            pageIndex,
            pageSize,
            sort: sort,
            filter: filter);

        return new PagedResult<Customer>(
            pageIndex,
            pageSize,
            documents.TotalCount,
            documents.Results
                .Select(x => x.ToEntity())
                .ToList());
    }

    public async Task<Customer> GetAsync(Guid id)
    {
        try
        {
            var document = await _repository.GetOneAsync(id);
            return document.ToEntity();
        }
        catch (Exception ex)
        {
            if (ex is Common.MongoClient.Exceptions.NotFoundException<CustomerDocument>)
                throw new NotFoundException(nameof(Customer), "ID", id.ToString());

            throw;
        }
    }

    public async Task<Customer?> GetOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document?.ToEntity();
    }

    public async Task UpdateAsync(Customer entity)
    {
        var document = CustomerDocument.From(entity);
        await _repository.UpdateAsync(document);
    }
}
