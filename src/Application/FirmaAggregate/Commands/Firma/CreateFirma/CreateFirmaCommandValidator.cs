using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.FirmaAggregate;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRMBackend.Application.FirmaAggregate.Commands.Firma.CreateFirma;

public class CreateFirmaCommandValidator : AbstractValidator<CreateFirmaCommand>
{
    private readonly IReadRepository<Domain.AggregateRoots.FirmaAggregate.Firma> _firmaRepository;

    public CreateFirmaCommandValidator(IReadRepository<Domain.AggregateRoots.FirmaAggregate.Firma> firmaRepository)
    {
        _firmaRepository = firmaRepository;

        RuleFor(v => v.ICO)
            .MustAsync(BeUniqueIco)
            .WithMessage("Firma s daným IČO už existuje.");
    }

    private async Task<bool> BeUniqueIco(string ico, CancellationToken cancellationToken)
    {
        return await _firmaRepository.GetQueryableNoTracking()
            .AllAsync(f => f.ICO != ico, cancellationToken);
    }
} 
