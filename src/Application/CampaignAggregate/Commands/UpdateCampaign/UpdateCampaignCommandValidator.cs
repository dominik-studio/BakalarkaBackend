using FluentValidation;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;

namespace PromobayBackend.Application.CampaignAggregate.Commands.UpdateCampaign;

public class UpdateCampaignCommandValidator : AbstractValidator<UpdateCampaignCommand>
{
    private readonly IReadRepository<Campaign> _repository;

    public UpdateCampaignCommandValidator(IReadRepository<Campaign> repository)
    {
        _repository = repository;

        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(v => v.Name)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name is required.")
            .MustAsync(BeUniqueTitle)
            .WithMessage("'{PropertyName}' must be unique.")
            .WithErrorCode("Unique");
    }

    private async Task<bool> BeUniqueTitle(UpdateCampaignCommand command, string? title, CancellationToken cancellationToken)
    {
        return !await _repository.GetQueryableNoTracking()
            .AnyAsync(c => c.Name == title && c.Id != command.Id, cancellationToken);
    }
} 
