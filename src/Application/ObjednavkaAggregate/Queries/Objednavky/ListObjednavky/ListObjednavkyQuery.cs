using AutoMapper;
using AutoMapper.QueryableExtensions;
using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Application.Common.Models;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plainquire.Filter;
using Plainquire.Filter.Abstractions;
using Plainquire.Page;
using Plainquire.Page.Abstractions;
using Plainquire.Sort;
using Plainquire.Sort.Abstractions;

namespace CRMBackend.Application.ObjednavkaAggregate.Queries.Objednavky.ListObjednavky;

public record ListObjednavkyQuery : IRequest<PaginatedList<ObjednavkaDTO>>
{
    public required EntityFilter<Objednavka> Filter { get; init; }
    public required EntitySort<Objednavka> Sort { get; init; }
    public required EntityPage Page { get; init; }
}

public class ListObjednavkyQueryHandler : IRequestHandler<ListObjednavkyQuery, PaginatedList<ObjednavkaDTO>>
{
    private readonly IReadRepository<Objednavka> _readRepository;
    private readonly IMapper _mapper;

    public ListObjednavkyQueryHandler(IReadRepository<Objednavka> readRepository, IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ObjednavkaDTO>> Handle(ListObjednavkyQuery request, CancellationToken cancellationToken)
    {
        var query = _readRepository.GetQueryableNoTracking()
            .Include(o => o.Firma)
            .Include(o => o.KontaktnaOsoba)
            .Where(request.Filter)
            .OrderBy(request.Sort)
            .ProjectTo<ObjednavkaDTO>(_mapper.ConfigurationProvider);

        return await PaginatedList<ObjednavkaDTO>.CreateAsync(query, request.Page, cancellationToken);
    }
} 
