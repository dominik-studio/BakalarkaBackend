using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.TovarAggregate;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRMBackend.Application.DodavatelAggregate.Commands.Tovary.CreateTovar;

public class CreateTovarCommandValidator : AbstractValidator<CreateTovarCommand>
{
    private readonly IReadRepository<Domain.AggregateRoots.TovarAggregate.Tovar> _tovarRepository;

    public CreateTovarCommandValidator(IReadRepository<Domain.AggregateRoots.TovarAggregate.Tovar> tovarRepository)
    {
        _tovarRepository = tovarRepository;

        RuleFor(v => v.InterneId)
            .MustAsync(BeUniqueInterneId)
            .WithMessage("Tovar s daným Interným ID už existuje.");
    }

    private async Task<bool> BeUniqueInterneId(string interneId, CancellationToken cancellationToken)
    {
        return await _tovarRepository.GetQueryableNoTracking()
            .AllAsync(t => t.InterneId != interneId, cancellationToken);
    }
} 
