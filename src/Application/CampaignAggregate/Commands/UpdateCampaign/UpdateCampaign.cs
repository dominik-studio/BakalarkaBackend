using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;
using PromobayBackend.Domain.Exceptions;

namespace PromobayBackend.Application.CampaignAggregate.Commands.UpdateCampaign;

public record UpdateCampaignCommand : IRequest
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? RewardJson { get; init; }
}

public class UpdateCampaignCommandHandler : IRequestHandler<UpdateCampaignCommand>
{
    private readonly IWriteRepository<Campaign> _repository;

    public UpdateCampaignCommandHandler(IWriteRepository<Campaign> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateCampaignCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Name);
        
        var campaign = await _repository.GetByIdAsync(request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, campaign);

        campaign.Name = request.Name;
        
        if (request.Description is not null)
        {
            campaign.SetDescription(request.Description);
        }
        
        if (request.RewardJson is not null)
        {
            campaign.SetReward(request.RewardJson);
        }

        _repository.Update(campaign);
        await _repository.SaveAsync(cancellationToken);
    }
} 
