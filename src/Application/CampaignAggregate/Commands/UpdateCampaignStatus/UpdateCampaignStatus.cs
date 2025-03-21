using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;
using PromobayBackend.Domain.Exceptions;
using PromobayBackend.Domain.Enums;

namespace PromobayBackend.Application.CampaignAggregate.Commands.UpdateCampaignStatus;

public record UpdateCampaignStatusCommand : IRequest
{
    public int Id { get; init; }
    public CampaignStatus? Status { get; init; }
}

public class UpdateCampaignStatusCommandHandler : IRequestHandler<UpdateCampaignStatusCommand>
{
    private readonly IWriteRepository<Campaign> _repository;

    public UpdateCampaignStatusCommandHandler(IWriteRepository<Campaign> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateCampaignStatusCommand request, CancellationToken cancellationToken)
    {
        var campaign = await _repository.GetByIdAsync(request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, campaign);

        switch (request.Status)
        {
            case CampaignStatus.Published:
                campaign.Publish();
                break;
            case CampaignStatus.Draft:
                campaign.SetToDraft();
                break;
            case CampaignStatus.Expired:
                throw new ValidationException("Campaign status cannot be manually set to Expired. It is automatically set when the campaign end date is reached.");
            default:
                throw new DomainValidationException("Invalid status transition.");
        }

        _repository.Update(campaign);
        await _repository.SaveAsync(cancellationToken);
    }
} 
