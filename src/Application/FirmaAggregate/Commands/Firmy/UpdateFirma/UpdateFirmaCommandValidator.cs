using CRMBackend.Application.Common.Interfaces.Repositories;

namespace CRMBackend.Application.FirmaAggregate.Commands.Firmy.UpdateFirma;

public class UpdateFirmaCommandValidator : AbstractValidator<UpdateFirmaCommand>
{
    private readonly IReadRepository<Domain.AggregateRoots.FirmaAggregate.Firma> _firmaRepository;

    public UpdateFirmaCommandValidator(IReadRepository<Domain.AggregateRoots.FirmaAggregate.Firma> firmaRepository)
    {
        _firmaRepository = firmaRepository;

        RuleFor(v => v.ICO)
            .MustAsync(BeUniqueIco)
            .WithMessage("Firma s daným IČO už existuje.");
    }

    private async Task<bool> BeUniqueIco(UpdateFirmaCommand command, string ico, CancellationToken cancellationToken)
    {
        return await _firmaRepository.GetQueryableNoTracking()
            .Where(f => f.Id != command.Id)
            .AllAsync(f => f.ICO != ico, cancellationToken);
    }
} 
