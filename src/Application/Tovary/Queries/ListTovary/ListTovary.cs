using AutoMapper;
using AutoMapper.QueryableExtensions;
using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plainquire.Filter;
using Plainquire.Page;
using Plainquire.Sort;
using DomainTovar = CRMBackend.Domain.AggregateRoots.TovarAggregate.Tovar;
using System.Linq.Expressions;

namespace CRMBackend.Application.Tovary.Queries.ListTovary;

public record ListTovaryQuery : IRequest<PaginatedList<TovarDTO>>
{
    public required EntityFilter<DomainTovar> Filter { get; init; }
    public required EntitySort<DomainTovar> Sort { get; init; }
    public required EntityPage Page { get; init; }
    public string? Search { get; init; }
}

public class ListTovaryQueryHandler : IRequestHandler<ListTovaryQuery, PaginatedList<TovarDTO>>
{
    private readonly IReadRepository<DomainTovar> _readRepository;
    private readonly IMapper _mapper;

    public ListTovaryQueryHandler(IReadRepository<DomainTovar> readRepository, IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TovarDTO>> Handle(ListTovaryQuery request, CancellationToken cancellationToken)
    {
        IQueryable<DomainTovar> query = _readRepository.GetQueryableNoTracking();

        query = query
            .Include(t => t.Dodavatel)
                .ThenInclude(d => d!.Adresa);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.Trim().ToLowerInvariant();

            query = query.Where(t =>
                (t.Nazov != null && t.Nazov.ToLowerInvariant().Contains(searchTerm)) ||
                (t.InterneId != null && t.InterneId.ToLowerInvariant().Contains(searchTerm)) ||
                (t.Ean != null && t.Ean.ToLowerInvariant().Contains(searchTerm)) ||
                (t.Dodavatel != null && t.Dodavatel.NazovFirmy != null && t.Dodavatel.NazovFirmy.ToLowerInvariant().Contains(searchTerm)) ||
                (t.Dodavatel != null && t.Dodavatel.Adresa != null && t.Dodavatel.Adresa.Ulica != null && t.Dodavatel.Adresa.Ulica.ToLowerInvariant().Contains(searchTerm)) ||
                (t.Dodavatel != null && t.Dodavatel.Email != null && t.Dodavatel.Email.ToLowerInvariant().Contains(searchTerm)) ||
                (t.Dodavatel != null && t.Dodavatel.Telefon != null && t.Dodavatel.Telefon.ToLowerInvariant().Contains(searchTerm))
            );
        }

        var orderedQuery = query
            .Where(request.Filter)
            .OrderBy(request.Sort);

        var projectedQuery = orderedQuery.ProjectTo<TovarDTO>(_mapper.ConfigurationProvider);

        return await PaginatedList<TovarDTO>.CreateAsync(projectedQuery, request.Page, cancellationToken);
    }
} 
