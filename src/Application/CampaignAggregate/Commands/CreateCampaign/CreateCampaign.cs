using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;
using PromobayBackend.Domain.Enums;
using PromobayBackend.Domain.AggregateRoots.BrandAggregate;

namespace PromobayBackend.Application.CampaignAggregate.Commands.CreateCampaign;

public record CreateCampaignCommand : IRequest<int>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? CampaignStart { get; init; }
    public DateTime? CampaignEnd { get; init; }
    public int BrandId { get; init; }
    public string? RewardJson { get; init; }
}

public class CreateCampaignCommandHandler : IRequestHandler<CreateCampaignCommand, int>
{
    private readonly IWriteRepository<Campaign> _repository;
    private readonly IWriteRepository<Brand> _brandRepository;

    public CreateCampaignCommandHandler(IWriteRepository<Campaign> repository, IWriteRepository<Brand> brandRepository)
    {
        _repository = repository;
        _brandRepository = brandRepository;
    }

    public async Task<int> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);
        Guard.Against.NotFound(request.BrandId, brand);

        var entity = new Campaign
        {
            Name = request.Name,
            Brand = brand,
        };

        if (request.Description is not null)
        {
            entity.SetDescription(request.Description);
        }

        if (request.CampaignStart.HasValue && request.CampaignEnd.HasValue)
        {
            entity.SetDates(request.CampaignStart.Value, request.CampaignEnd.Value);
        }

        if (request.RewardJson is not null)
        {
            entity.SetReward(request.RewardJson);
        }

        _repository.Add(entity);
        await _repository.SaveAsync(cancellationToken);
        
        return entity.Id;
    }
} 
