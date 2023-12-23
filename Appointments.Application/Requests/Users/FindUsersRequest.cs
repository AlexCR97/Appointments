using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Users;

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
