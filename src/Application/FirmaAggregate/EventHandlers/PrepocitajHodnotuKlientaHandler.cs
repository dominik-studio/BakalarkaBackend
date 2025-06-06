using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots;
using CRMBackend.Domain.AggregateRoots.FirmaAggregate;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;
using CRMBackend.Domain.Entities;
using CRMBackend.Domain.Enums;
using CRMBackend.Domain.Events;
using Microsoft.Extensions.Logging;

namespace CRMBackend.Application.Firmy.EventHandlers;

public class PrepocitajHodnotuKlientaHandler : INotificationHandler<ObjednavkaVybavenaEvent>
{
    private readonly ILogger<PrepocitajHodnotuKlientaHandler> _logger;
    private readonly IWriteRepository<Firma> _firmaWriteRepository;

    private const int PocetRokovHistoriaObjednavok = 5;
    private const decimal MaxKoeficientExtrapolacie = 5m;
    private const decimal KoeficientHodnotaExtrapolacie = 0.5m;

    public PrepocitajHodnotuKlientaHandler(
        ILogger<PrepocitajHodnotuKlientaHandler> logger,
        IWriteRepository<Firma> firmaWriteRepository)
    {
        _logger = logger;
        _firmaWriteRepository = firmaWriteRepository;
    }

    private async Task PrepocitajHodnotu(int firmaId, CancellationToken cancellationToken)
    {
        var firma = await _firmaWriteRepository.GetByIdWithIncludesAsync(
            firmaId,
            query => query
                .Include(f => f.Objednavky)
                    .ThenInclude(o => o.PoslednaCenovaPonuka),
            cancellationToken);

        if (firma == null)
        {
            _logger.LogWarning("Firma s ID {FirmaId} neexistuje", firmaId);
            return;
        }

        var datumOd = DateTime.UtcNow.AddYears(-PocetRokovHistoriaObjednavok);

        var objednavky = firma.Objednavky
            .Where(o => o.VytvoreneDna >= datumOd && o.Faza == ObjednavkaFaza.Vybavene)
            .ToList();

        if (!objednavky.Any())
        {
            firma.UpdateHodnotaObjednavok(0);
            await _firmaWriteRepository.SaveAsync(cancellationToken);
            return;
        }
        
        decimal hodnotaObjednavok = objednavky
            .Sum(o => o.PoslednaCenovaPonuka?.FinalnaCena ?? 0);

        var datumRegistracieFirmy = firma.VytvoreneDna;
        var vekFirmyMesiace = (DateTime.UtcNow - datumRegistracieFirmy).TotalDays / 30;
        vekFirmyMesiace = Math.Min(1, vekFirmyMesiace);

        if (vekFirmyMesiace < PocetRokovHistoriaObjednavok * 12)
        {
            decimal faktorExtrapolacie = Math.Min(MaxKoeficientExtrapolacie, (PocetRokovHistoriaObjednavok * 12) / (decimal)vekFirmyMesiace);
            faktorExtrapolacie = Math.Max(1, faktorExtrapolacie);
                
            decimal extrapolovanaHodnota = hodnotaObjednavok * faktorExtrapolacie;
            decimal chybajucaCast = extrapolovanaHodnota - hodnotaObjednavok;
            hodnotaObjednavok = hodnotaObjednavok + (chybajucaCast * KoeficientHodnotaExtrapolacie);
        }

        firma.UpdateHodnotaObjednavok(hodnotaObjednavok);
        await _firmaWriteRepository.SaveAsync(cancellationToken);
    }

    public async Task Handle(ObjednavkaVybavenaEvent notification, CancellationToken cancellationToken)
    {
        await PrepocitajHodnotu(notification.FirmaId, cancellationToken);
    }
} 
