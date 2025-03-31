using AutoMapper;
using AutoMapper.QueryableExtensions;
using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Application.Common.Models;
using CRMBackend.Domain.AggregateRoots.FirmaAggregate;
using MediatR;
using Plainquire.Filter;
using Plainquire.Filter.Abstractions;
using Plainquire.Page;
using Plainquire.Page.Abstractions;
using Plainquire.Sort;
using Plainquire.Sort.Abstractions;

namespace CRMBackend.Application.FirmaAggregate.Queries.ListFirmy;

public record ListFirmyQuery : IRequest< PaginatedList<FirmaDTO> >
{
    public required EntityFilter<Firma> Filter { get; init; }
    public required EntitySort<Firma> Sort { get; init; }
    public required EntityPage Page { get; init; }
}

public class ListFirmyQueryHandler : IRequestHandler< ListFirmyQuery, PaginatedList<FirmaDTO> >
{
    private readonly IReadRepository<Firma> _repository;
    private readonly IMapper _mapper;

    public ListFirmyQueryHandler(IReadRepository<Firma> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task< PaginatedList<FirmaDTO> > Handle(ListFirmyQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryableNoTracking()
            .Where(request.Filter)
            .OrderBy(request.Sort)
            .ProjectTo<FirmaDTO>(_mapper.ConfigurationProvider);

        return await PaginatedList<FirmaDTO>.CreateAsync(query, request.Page, cancellationToken);
    }
}
