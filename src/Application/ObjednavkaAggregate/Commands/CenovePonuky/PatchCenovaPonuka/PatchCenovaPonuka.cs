using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;
using CRMBackend.Domain.AggregateRoots.TovarAggregate;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Ardalis.GuardClauses;

namespace CRMBackend.Application.ObjednavkaAggregate.Commands.CenovePonuky.PatchCenovaPonuka
{
    public record PatchCenovaPonukaCommand : IRequest
    {
        public required int ObjednavkaId { get; init; }
        public required int CenovaPonukaId { get; init; }
        public decimal? FinalnaCena { get; init; }
        public List<CenovaPonukaTovarCommandDto>? Polozky { get; init; }
    }

    public class PatchCenovaPonukaCommandHandler : IRequestHandler<PatchCenovaPonukaCommand>
    {
        private readonly IWriteRepository<Objednavka> _objednavkaRepository;
        private readonly IReadRepository<Tovar> _tovarRepository;
        private readonly IReadRepository<VariantTovar> _variantTovarRepository;

        public PatchCenovaPonukaCommandHandler(IWriteRepository<Objednavka> objednavkaRepository,
            IReadRepository<Tovar> tovarRepository,
            IReadRepository<VariantTovar> variantTovarRepository)
        {
            _objednavkaRepository = objednavkaRepository;
            _tovarRepository = tovarRepository;
            _variantTovarRepository = variantTovarRepository;
        }

        public async Task Handle(PatchCenovaPonukaCommand request, CancellationToken cancellationToken)
        {
            var objednavka = await _objednavkaRepository.GetByIdWithIncludesAsync(
                request.ObjednavkaId,
                query => query.Include(o => o.CenovePonuky).ThenInclude(cp => cp.Polozky),
                cancellationToken);

            Guard.Against.NotFound(request.ObjednavkaId, objednavka);

            var cenovaPonuka = objednavka.CenovePonuky.FirstOrDefault(x => x.Id == request.CenovaPonukaId);
            Guard.Against.NotFound(request.CenovaPonukaId, cenovaPonuka);

            bool updated = false;

            if (request.FinalnaCena.HasValue)
            {
                cenovaPonuka.SetFinalnaCena(request.FinalnaCena.Value);
                updated = true;
            }

            if (request.Polozky is not null)
            {
                var existingItems = cenovaPonuka.Polozky.ToList();
                foreach (var item in existingItems)
                {
                    cenovaPonuka.RemovePolozka(item.Id);
                }

                foreach (var polozkaDto in request.Polozky)
                {
                    Tovar? tovar = null;
                    VariantTovar? variantTovar = null;

                    if (polozkaDto.TovarId.HasValue && polozkaDto.VariantTovarId.HasValue)
                    {
                        variantTovar = await _variantTovarRepository.GetQueryableNoTracking()
                            .Include(v => v.Tovar)
                            .FirstOrDefaultAsync(v => v.Id == polozkaDto.VariantTovarId.Value, cancellationToken);
                        Guard.Against.NotFound(polozkaDto.VariantTovarId.Value, variantTovar);

                        tovar = variantTovar.Tovar;
                        Guard.Against.NotFound(polozkaDto.VariantTovarId.Value, tovar);
                    }
                    else if (polozkaDto.TovarId.HasValue)
                    {
                        tovar = await _tovarRepository.GetQueryableNoTracking()
                            .FirstOrDefaultAsync(t => t.Id == polozkaDto.TovarId.Value, cancellationToken);
                        Guard.Against.NotFound(polozkaDto.TovarId.Value, tovar);
                    }
                    else
                    {
                        throw new ArgumentException("Pre položku cenovej ponuky musí byť zadané TovarId.");
                    }

                    var novaPolozka = new CenovaPonukaTovar(tovar, variantTovar)
                    {
                        Mnozstvo = polozkaDto.Mnozstvo
                    };
                    cenovaPonuka.AddPolozka(novaPolozka);
                }
                updated = true;
            }

            if (updated)
            {
                await _objednavkaRepository.SaveAsync(cancellationToken);
            }
        }
    }
} 
