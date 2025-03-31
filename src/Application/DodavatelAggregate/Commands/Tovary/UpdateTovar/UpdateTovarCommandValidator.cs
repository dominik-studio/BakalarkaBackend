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
    }

    private async Task<bool> BeUniqueInterneId(UpdateTovarCommand command, string interneId, CancellationToken cancellationToken)
    {
        return await _tovarRepository.GetQueryableNoTracking()
            .Where(t => t.Id != command.TovarId)
            .AllAsync(t => t.InterneId != interneId, cancellationToken);
    }
} 
