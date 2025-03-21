using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;
using PromobayBackend.Domain.Exceptions;

namespace PromobayBackend.Application.CampaignAggregate.Commands.UpdateCampaignDates;

public record UpdateCampaignDatesCommand : IRequest
{
    public required int Id { get; init; }
    public required DateTime Start { get; init; }
    public required DateTime End { get; init; }
}

public class UpdateCampaignDatesCommandHandler : IRequestHandler<UpdateCampaignDatesCommand>
{
    private readonly IWriteRepository<Campaign> _repository;

    public UpdateCampaignDatesCommandHandler(IWriteRepository<Campaign> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateCampaignDatesCommand request, CancellationToken cancellationToken)
    {
        var campaign = await _repository.GetByIdAsync(request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, campaign);

        campaign.SetDates(request.Start, request.End);
        _repository.Update(campaign);
        await _repository.SaveAsync(cancellationToken);
    }
} 
