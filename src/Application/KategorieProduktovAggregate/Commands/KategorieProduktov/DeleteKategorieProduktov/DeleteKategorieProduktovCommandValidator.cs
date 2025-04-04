using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;
using CRMBackend.Domain.AggregateRoots.TovarAggregate;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRMBackend.Application.KategorieProduktovAggregate.Commands.KategorieProduktov.DeleteKategorieProduktov;

public class DeleteKategorieProduktovCommandValidator : AbstractValidator<DeleteKategorieProduktovCommand>
{
    private readonly IReadRepository<Tovar> _tovarRepository;

    public DeleteKategorieProduktovCommandValidator(IReadRepository<Tovar> tovarRepository)
    {
        _tovarRepository = tovarRepository;

        RuleFor(v => v.Id)
            .MustAsync(NotBeReferencedByTovar)
            .WithMessage("Kategóriu nie je možné odstrániť, pretože sa nachádza v existujúcej cenovej ponuke.");
    }

    private async Task<bool> NotBeReferencedByTovar(int kategoriaId, CancellationToken cancellationToken)
    {
        bool exists = await _tovarRepository.GetQueryableNoTracking()
            .AnyAsync(t => t.KategoriaId == kategoriaId, cancellationToken);

        return !exists;
    }
} 