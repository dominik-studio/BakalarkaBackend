using FluentValidation;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;

namespace PromobayBackend.Application.CampaignAggregate.Commands.CreateCampaign;

public class CreateCampaignCommandValidator : AbstractValidator<CreateCampaignCommand>
{
    private readonly IReadRepository<Campaign> _repository;

    public CreateCampaignCommandValidator(IReadRepository<Campaign> repository)
    {
        _repository = repository;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MustAsync(BeUniqueTitle)
            .WithMessage("'{PropertyName}' must be unique.")
            .WithErrorCode("Unique");
    }

    private async Task<bool> BeUniqueTitle(string? title, CancellationToken cancellationToken)
    {
        return !await _repository.GetQueryableNoTracking()
            .AnyAsync(c => c.Name == title, cancellationToken);
    }
} 