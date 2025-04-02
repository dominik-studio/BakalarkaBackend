using AutoMapper;
using AutoMapper.QueryableExtensions;
using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Application.Common.Models;
using MediatR;
using Plainquire.Filter;
using Plainquire.Filter.Abstractions;
using Plainquire.Page.Abstractions;
using Plainquire.Sort.Abstractions;
using Plainquire.Page;
using Plainquire.Sort;

namespace CRMBackend.Application.Tovar.Queries.ListTovary;

public record ListTovaryQuery : IRequest<PaginatedList<TovarDTO>>
{
    public required EntityFilter<Domain.AggregateRoots.TovarAggregate.Tovar> Filter { get; init; }
    public required EntitySort<Domain.AggregateRoots.TovarAggregate.Tovar> Sort { get; init; }
    public required EntityPage Page { get; init; }
}

public class ListTovaryQueryHandler : IRequestHandler<ListTovaryQuery, PaginatedList<TovarDTO>>
{
    private readonly IReadRepository<Domain.AggregateRoots.TovarAggregate.Tovar> _readRepository;
    private readonly IMapper _mapper;

    public ListTovaryQueryHandler(IReadRepository<Domain.AggregateRoots.TovarAggregate.Tovar> readRepository, IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TovarDTO>> Handle(ListTovaryQuery request, CancellationToken cancellationToken)
    {
        var query = _readRepository.GetQueryableNoTracking()
            .Where(request.Filter)
            .OrderBy(request.Sort)
            .ProjectTo<TovarDTO>(_mapper.ConfigurationProvider);

        return await PaginatedList<TovarDTO>.CreateAsync(query, request.Page, cancellationToken);
    }
} 
