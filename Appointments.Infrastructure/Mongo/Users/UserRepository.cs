using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Core.Application.Requests.Users;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Infrastructure.Mongo.Users;

internal sealed class UserRepository : IUserRepository
{
    private readonly IMongoRepository<UserDocument> _repository;

    public UserRepository(IMongoRepository<UserDocument> repository)
    {
        _repository = repository;
    }

    public async Task<User> CreateAsync(User entity)
    {
        var document = UserDocument.From(entity);
        var createdDocument = await _repository.CreateAsync(document);
        return createdDocument.ToEntity();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        var user = await GetByEmailOrDefaultAsync(email);
        return user is not null;
    }

    public async Task<PagedResult<User>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        var documents = await _repository.GetAsync(
            pageIndex,
            pageSize,
            sort: sort,
            filter: filter);

        return new PagedResult<User>(
            pageIndex,
            pageSize,
            documents.TotalCount,
            documents.Results
                .Select(x => x.ToEntity())
                .ToList());
    }

    public async Task<User> GetAsync(Guid id)
    {
        try
        {
            var document = await _repository.GetOneAsync(id);
            return document.ToEntity();
        }
        catch (Exception ex)
        {
            if (ex is Common.MongoClient.Exceptions.NotFoundException<UserDocument>)
                throw new NotFoundException(nameof(User), "ID", id.ToString());

            throw;
        }
    }

    public async Task<User?> GetByEmailOrDefaultAsync(Email email)
    {
        var emailValue = email.Value;
        var localIdentityProvider = IdentityProvider.Local.ToString();

        var document = await _repository.GetOneOrDefaultAsync(x => x.Logins
            .Any(login => true
                && login.IdentityProvider == localIdentityProvider
                && login.Email == emailValue));

        return document?.ToEntity();
    }

    public async Task<User?> GetOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document?.ToEntity();
    }

    public async Task UpdateAsync(User entity)
    {
        var document = UserDocument.From(entity);
        await _repository.UpdateAsync(document);
    }
}
