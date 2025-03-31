using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRMBackend.Application.DodavatelAggregate.Commands.Tovary.DeleteTovar;


public class DeleteTovarCommandValidator : AbstractValidator<DeleteTovarCommand>
{
    private readonly IReadRepository<CenovaPonukaTovar> _cenovaPonukaPolozkaRepository;

    public DeleteTovarCommandValidator(IReadRepository<CenovaPonukaTovar> cenovaPonukaPolozkaRepository)
    {
        _cenovaPonukaPolozkaRepository = cenovaPonukaPolozkaRepository;

        RuleFor(v => v.TovarId)
            .MustAsync(NotBeReferencedInCenovaPonukaPolozka)
            .WithMessage("Tovar nie je možné odstrániť, pretože sa nachádza v existujúcej cenovej ponuke.");
    }

    private async Task<bool> NotBeReferencedInCenovaPonukaPolozka(int tovarId, CancellationToken cancellationToken)
    {
        bool exists = await _cenovaPonukaPolozkaRepository.GetQueryableNoTracking()
            .AnyAsync(p => p.TovarId == tovarId, cancellationToken);

        return !exists;
    }
} 
