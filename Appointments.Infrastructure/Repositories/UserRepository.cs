﻿using Appointments.Application.Extensions.Normalization;
using Appointments.Application.Repositories.Users;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Mapper.Abstractions;
using Appointments.Infrastructure.Mongo.Documents;

namespace Appointments.Infrastructure.Repositories;

internal class UserRepository : EntityRepository<User, UserDocument>, IUserRepository
{
    public UserRepository(IMapper mapper, IMongoRepository<UserDocument> repository) : base(mapper, repository)
    {
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        var normalizedEmail = email.NormalizeForComparison();
        var document = await _repository.GetOneOrDefaultAsync(x => x.Email == normalizedEmail);
        return document is not null;
    }
}