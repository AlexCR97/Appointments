using Appointments.Application.Mapper.Abstractions;
using Appointments.Application.Repositories.Abstractions;
using Appointments.Application.Validations;
using Appointments.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests;

public abstract record GetPagedRequest<T>(
    int PageIndex,
    int PageSize,
    string? Sort,
    string? Filter) : IRequest<PagedResult<T>>
{
    public const int MaxPageSize = 100;
};

internal class GetPagedRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, PagedResult<TModel>>
    where TRequest : GetPagedRequest<TModel>
{
    private readonly IMapper _mapper;
    private readonly IRepository<TEntity> _repository;

    public GetPagedRequestHandler(IMapper mapper, IRepository<TEntity> serviceRepository)
    {
        _mapper = mapper;
        _repository = serviceRepository;
    }

    public async Task<PagedResult<TModel>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        new GetPagedRequestValidator<TModel>().ValidateAndThrow(request);

        var pagedResult = await _repository.GetAsync(
            request.PageIndex,
            request.PageSize,
            request.Sort,
            request.Filter);

        return new PagedResult<TModel>(
            pagedResult.PageIndex,
            pagedResult.PageSize,
            pagedResult.TotalCount,
            pagedResult.Results
                .Select(x => _mapper.Map<TModel>(x!))
                .ToList());
    }
}
