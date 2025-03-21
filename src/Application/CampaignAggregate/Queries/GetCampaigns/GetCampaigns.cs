using Plainquire.Filter;
using Plainquire.Page;
using Plainquire.Sort;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Application.Common.Mappings;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;

namespace PromobayBackend.Application.CampaignAggregate.Queries.GetCampaigns;

public record GetCampaignsQuery : IRequest<IReadOnlyCollection<CampaignDto>>
{
    public required EntityFilter<Campaign> Filter { get; init; }
    public required EntitySort<Campaign> Sort { get; init; }
    public required EntityPage<Campaign> Page { get; init; }
}

public class GetCampaignsQueryHandler : IRequestHandler<GetCampaignsQuery, IReadOnlyCollection<CampaignDto>>
{
    private readonly IReadRepository<Campaign> _repository;
    private readonly IMapper _mapper;

    public GetCampaignsQueryHandler(IReadRepository<Campaign> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<CampaignDto>> Handle(GetCampaignsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetQueryableNoTracking()
            .Where(request.Filter)
            .OrderBy(request.Sort)
            .Page(request.Page)
            .ProjectTo<CampaignDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
} 