using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;

namespace PromobayBackend.Application.CampaignAggregate.Commands.DeleteCampaign;

public record DeleteCampaignCommand(int Id) : IRequest;

public class DeleteCampaignCommandHandler : IRequestHandler<DeleteCampaignCommand>
{
    private readonly IWriteRepository<Campaign> _repository;

    public DeleteCampaignCommandHandler(IWriteRepository<Campaign> repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await _repository.GetByIdAsync(request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, campaign);

        _repository.Delete(campaign);
        await _repository.SaveAsync(cancellationToken);
    }
} 