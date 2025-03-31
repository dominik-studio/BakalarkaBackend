using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRMBackend.Application.FirmaAggregate.Commands.KontaktneOsoby.DeleteKontaktnaOsoba;

public record DeleteKontaktnaOsobaCommand : IRequest
{
    public required int KontaktnaOsobaId { get; init; }
}


public class DeleteKontaktnaOsobaCommandValidator : AbstractValidator<DeleteKontaktnaOsobaCommand>
{
    private readonly IReadRepository<Objednavka> _objednavkaRepository;

    public DeleteKontaktnaOsobaCommandValidator(IReadRepository<Objednavka> objednavkaRepository)
    {
        _objednavkaRepository = objednavkaRepository;

        RuleFor(v => v.KontaktnaOsobaId)
            .MustAsync(NotBeReferencedInObjednavka)
            .WithMessage("Kontaktnú osobu nie je možné odstrániť, pretože je priradená k existujúcej objednávke.");
    }

    private async Task<bool> NotBeReferencedInObjednavka(int kontaktnaOsobaId, CancellationToken cancellationToken)
    {
        bool exists = await _objednavkaRepository.GetQueryableNoTracking()
            .AnyAsync(o => o.KontaktnaOsobaId == kontaktnaOsobaId, cancellationToken);

        return !exists;
    }
} 
