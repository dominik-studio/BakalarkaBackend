using Ardalis.GuardClauses;
using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;
using CRMBackend.Domain.AggregateRoots.TovarAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRMBackend.Application.ObjednavkaAggregate.Commands.CenovePonuky.CreateCenovaPonuka;

public record CreateCenovaPonukaCommand : IRequest<int>
{
    public required int ObjednavkaId { get; init; }
    public required decimal FinalnaCena { get; init; }
    public required List<CenovaPonukaTovarCommandDto> Polozky { get; init; }
}

public class CreateCenovaPonukaCommandHandler : IRequestHandler<CreateCenovaPonukaCommand, int>
{
    private readonly IWriteRepository<Objednavka> _objednavkaRepository;
    private readonly IReadRepository<Tovar> _tovarRepository;
    private readonly IReadRepository<VariantTovar> _variantTovarRepository;

    public CreateCenovaPonukaCommandHandler(
        IWriteRepository<Objednavka> objednavkaRepository,
        IReadRepository<Tovar> tovarRepository,
        IReadRepository<VariantTovar> variantTovarRepository)
    {
        _objednavkaRepository = objednavkaRepository;
        _tovarRepository = tovarRepository;
        _variantTovarRepository = variantTovarRepository;
    }

    public async Task<int> Handle(CreateCenovaPonukaCommand request, CancellationToken cancellationToken)
    {
        var objednavka = await _objednavkaRepository.GetByIdWithIncludesAsync(
            request.ObjednavkaId,
            query => query.Include(o => o.CenovePonuky),
            cancellationToken);

        Guard.Against.NotFound(request.ObjednavkaId, objednavka);

        var novaCenovaPonuka = new CenovaPonuka
        {
            Objednavka = objednavka,
        };

        novaCenovaPonuka.SetFinalnaCena(request.FinalnaCena);

        foreach (var polozkaDto in request.Polozky)
        {
            Tovar? tovar = null;
            VariantTovar? variantTovar = null;
            if(polozkaDto.TovarId.HasValue)
                tovar = await _tovarRepository.GetQueryableNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == polozkaDto.TovarId.Value, cancellationToken);
            else if(polozkaDto.VariantTovarId.HasValue)
                variantTovar = await _variantTovarRepository.GetQueryableNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == polozkaDto.VariantTovarId.Value, cancellationToken);

            var polozka = new CenovaPonukaTovar(tovar, variantTovar)
            {
                Mnozstvo = polozkaDto.Mnozstvo
            };

            novaCenovaPonuka.AddPolozka(polozka);
        }
        
        objednavka.AddCenovaPonuka(novaCenovaPonuka);

        await _objednavkaRepository.SaveAsync(cancellationToken);
        return novaCenovaPonuka.Id;
    }
} 
