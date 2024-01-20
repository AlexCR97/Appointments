using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Core.Application.Requests.Appointments;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Infrastructure.Mongo.Appointments;

internal sealed class AppointmentRepository : IAppointmentRepository
{
    private readonly IMongoRepository<AppointmentDocument> _repository;

    public AppointmentRepository(IMongoRepository<AppointmentDocument> repository)
    {
        _repository = repository;
    }

    public async Task<Appointment> CreateAsync(Appointment entity)
    {
        var document = AppointmentDocument.From(entity);
        var createdDocument = await _repository.CreateAsync(document);
        return createdDocument.ToEntity();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PagedResult<Appointment>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        var documents = await _repository.GetAsync(
            pageIndex,
            pageSize,
            sort: sort,
            filter: filter);

        return new PagedResult<Appointment>(
            pageIndex,
            pageSize,
            documents.TotalCount,
            documents.Results
                .Select(x => x.ToEntity())
                .ToList());
    }

    public async Task<Appointment> GetAsync(Guid id)
    {
        try
        {
            var document = await _repository.GetOneAsync(id);
            return document.ToEntity();
        }
        catch (Exception ex)
        {
            if (ex is Common.MongoClient.Exceptions.NotFoundException<AppointmentDocument>)
                throw new NotFoundException(nameof(Appointment), "ID", id.ToString());

            throw;
        }
    }

    public async Task<Appointment?> GetOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document?.ToEntity();
    }

    public async Task UpdateAsync(Appointment entity)
    {
        var document = AppointmentDocument.From(entity);
        await _repository.UpdateAsync(document);
    }
}
