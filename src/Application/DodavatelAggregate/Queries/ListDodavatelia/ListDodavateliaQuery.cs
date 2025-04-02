using AutoMapper;
using AutoMapper.QueryableExtensions;
using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Application.Common.Models;
using CRMBackend.Domain.AggregateRoots.DodavatelAggregate;
using MediatR;
using Plainquire.Filter;
using Plainquire.Filter.Abstractions;
using Plainquire.Page;
using Plainquire.Page.Abstractions;
using Plainquire.Sort;
using Plainquire.Sort.Abstractions;

namespace CRMBackend.Application.DodavatelAggregate.Queries.ListDodavatelia;

public record ListDodavateliaQuery : IRequest<PaginatedList<DodavatelDTO>>
{
    public required EntityFilter<Dodavatel> Filter { get; init; }
    public required EntitySort<Dodavatel> Sort { get; init; }
    public required EntityPage Page { get; init; }
}

public class ListDodavateliaQueryHandler : IRequestHandler<ListDodavateliaQuery, PaginatedList<DodavatelDTO>>
{
    private readonly IReadRepository<Dodavatel> _readRepository;
    private readonly IMapper _mapper;

    public ListDodavateliaQueryHandler(IReadRepository<Dodavatel> readRepository, IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DodavatelDTO>> Handle(ListDodavateliaQuery request, CancellationToken cancellationToken)
    {
        var query = _readRepository.GetQueryableNoTracking()
            .Where(request.Filter)
            .OrderBy(request.Sort)
            .ProjectTo<DodavatelDTO>(_mapper.ConfigurationProvider);
        
        return await PaginatedList<DodavatelDTO>.CreateAsync(query, request.Page, cancellationToken);
    }
} 
