using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.TovarAggregate;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRMBackend.Application.DodavatelAggregate.Commands.Tovary.UpdateTovar;

public class UpdateTovarCommandValidator : AbstractValidator<UpdateTovarCommand>
{
    private readonly IReadRepository<Domain.AggregateRoots.TovarAggregate.Tovar> _tovarRepository;

    public UpdateTovarCommandValidator(IReadRepository<Domain.AggregateRoots.TovarAggregate.Tovar> tovarRepository)
    {
        _tovarRepository = tovarRepository;

        RuleFor(v => v.InterneId)
            .MustAsync(BeUniqueInterneId)
            .WithMessage("Tovar s daným Interným ID už existuje.");

        RuleFor(v => v.Ean)
            .MustAsync(BeUniqueEan)
            .WithMessage("Tovar s daným EAN už existuje.");
    }

    private async Task<bool> BeUniqueInterneId(UpdateTovarCommand command, string interneId, CancellationToken cancellationToken)
    {
        return await _tovarRepository.GetQueryableNoTracking()
            .Where(t => t.Id != command.TovarId)
            .AllAsync(t => t.InterneId != interneId, cancellationToken);
    }

    private async Task<bool> BeUniqueEan(UpdateTovarCommand command, string? ean, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(ean))
            return true;

        return await _tovarRepository.GetQueryableNoTracking()
            .Where(t => t.Id != command.TovarId)
            .AllAsync(t => t.Ean != ean, cancellationToken);
    }
} 
