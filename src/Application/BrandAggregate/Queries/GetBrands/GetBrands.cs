using Plainquire.Filter;
using Plainquire.Page;
using Plainquire.Sort;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Application.Common.Mappings;
using PromobayBackend.Domain.AggregateRoots.BrandAggregate;

namespace PromobayBackend.Application.BrandAggregate.Queries.GetBrands;

public record GetBrandsQuery : IRequest<IReadOnlyCollection<BrandDto>>
{
    public required EntityFilter<Brand> Filter { get; init; }
    public required EntitySort<Brand> Sort { get; init; }
    public required EntityPage<Brand> Page { get; init; }
}

public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, IReadOnlyCollection<BrandDto>>
{
    private readonly IReadRepository<Brand> _repository;
    private readonly IMapper _mapper;

    public GetBrandsQueryHandler(IReadRepository<Brand> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetQueryableNoTracking()
            .Where(request.Filter)
            .OrderBy(request.Sort)
            .Page(request.Page)
            .ProjectTo<BrandDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
} 