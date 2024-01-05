using Appointments.Common.Application;
using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Users;

public sealed record FindUsersRequest : FindRequest<User>
{
    public FindUsersRequest(int PageIndex, int PageSize, string? Sort, string? Filter) : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal sealed class GetUsersRequestHandler : IRequestHandler<FindUsersRequest, PagedResult<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResult<User>> Handle(FindUsersRequest request, CancellationToken cancellationToken)
    {
        new FindRequestValidator<User>().ValidateAndThrow(request);

        return await _userRepository.FindAsync(
            request.PageIndex,
            request.PageSize,
            request.Sort,
            request.Filter);
    }
}
