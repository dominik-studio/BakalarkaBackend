using CRMBackend.Application.Common.Interfaces.Data;
using CRMBackend.Domain.AggregateRoots;
using CRMBackend.Domain.Entities;
using CRMBackend.Domain.Enums;
using CRMBackend.Domain.Events;
using Microsoft.Extensions.Logging;

namespace CRMBackend.Application.Firmy.EventHandlers;

public class PrepocitajHodnotuKlientaHandler : INotificationHandler<ObjednavkaVybavenaEvent>
{
    private readonly ILogger<PrepocitajHodnotuKlientaHandler> _logger;
    private readonly IWriteRepository<Firma> _firmaWriteRepository;
    private readonly IReadRepository<Objednavka> _objednavkaReadRepository;

    private const int PocetRokovNaHistoriu = 5;
    private const decimal KoeficientPreNovehoklienta = 0.7m;

    public PrepocitajHodnotuKlientaHandler(
        ILogger<PrepocitajHodnotuKlientaHandler> logger,
        IWriteRepository<Firma> firmaWriteRepository,
        IReadRepository<Objednavka> objednavkaReadRepository)
    {
        _logger = logger;
        _firmaWriteRepository = firmaWriteRepository;
        _objednavkaReadRepository = objednavkaReadRepository;
    }

    private async Task PrepocitajHodnotu(int firmaId, CancellationToken cancellationToken)
    {
        var firma = await _firmaWriteRepository.GetByIdAsync(firmaId, cancellationToken);

        if (firma == null)
        {
            _logger.LogWarning("Firma s ID {FirmaId} neexistuje", firmaId);
            return;
        }

        var datumOd = DateTime.UtcNow.AddYears(-PocetRokovNaHistoriu);

        var objednavky = await _objednavkaReadRepository
            .GetQueryableNoTracking()
            .Where(o => o.FirmaId == firmaId && o.Created >= datumOd && o.Faza == ObjednavkaFaza.Vybavene)
            .ToListAsync(cancellationToken);

        if (!objednavky.Any())
        {
            firma.UpdateHodnotaObjednavok(0);
            await _firmaWriteRepository.SaveAsync(cancellationToken);
            return;
        }
        
        decimal hodnotaObjednavok = objednavky
            .Sum(o =>
            {
                if (o.PoslednaCenovaPonuka == null)
                {
                    _logger.LogWarning("Vybavená objednávka {ObjednavkaId} neobsahuje poslednú cenovú ponuku", o.Id);
                    return 0;
                }
                return o.PoslednaCenovaPonuka.FinalnaCena;
            });

        var datumRegistracieFirmy = firma.Created;
        var vekFirmyVMesiacoch = (DateTime.UtcNow - datumRegistracieFirmy).TotalDays / 30;

        if (vekFirmyVMesiacoch < PocetRokovNaHistoriu * 12)
        {
            if (vekFirmyVMesiacoch > 0)
            {
                decimal fullExtrapolatedValue = hodnotaObjednavok * ((PocetRokovNaHistoriu * 12) / (decimal)vekFirmyVMesiacoch);
                decimal missingPart = fullExtrapolatedValue - hodnotaObjednavok;
                hodnotaObjednavok = hodnotaObjednavok + (missingPart * KoeficientPreNovehoklienta);
            }
        }

        firma.UpdateHodnotaObjednavok(hodnotaObjednavok);
        await _firmaWriteRepository.SaveAsync(cancellationToken);

        _logger.LogInformation("Aktualizovaná hodnota objednávok pre firmu {FirmaId}: {Hodnota}", firmaId, hodnotaObjednavok);
    }

    public async Task Handle(ObjednavkaVybavenaEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);
        await PrepocitajHodnotu(notification.FirmaId, cancellationToken);
    }
} 
