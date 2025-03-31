using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Application.DodavatelAggregate.Commands.Tovary.DeleteTovar;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;

namespace CRMBackend.Application.DodavatelAggregate.Commands.VariantyTovarov.DeleteVariantTovar;


public class DeleteVariantTovarCommandValidator : AbstractValidator<DeleteVariantTovarCommand>
{
    private readonly IReadRepository<CenovaPonukaTovar> _cenovaPonukaPolozkaRepository;

    public DeleteVariantTovarCommandValidator(IReadRepository<CenovaPonukaTovar> cenovaPonukaPolozkaRepository)
    {
        _cenovaPonukaPolozkaRepository = cenovaPonukaPolozkaRepository;

        RuleFor(v => v.VariantId)
            .MustAsync(NotBeReferencedInCenovaPonukaPolozka)
            .WithMessage("Variant tovaru nie je možné odstrániť, pretože sa nachádza v existujúcej cenovej ponuke.");
    }

    private async Task<bool> NotBeReferencedInCenovaPonukaPolozka(int variantTovarId, CancellationToken cancellationToken)
    {
        bool exists = await _cenovaPonukaPolozkaRepository.GetQueryableNoTracking()
            .AnyAsync(p => p.VariantTovarId == variantTovarId, cancellationToken);

        return !exists;
    }
} 
