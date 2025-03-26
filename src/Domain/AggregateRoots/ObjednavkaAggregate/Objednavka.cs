using CRMBackend.Domain.Common;
using CRMBackend.Domain.Exceptions;

namespace CRMBackend.Domain.AggregateRoots;

public class Objednavka : BaseAuditableEntity, IAggregateRoot
{
    public required KontaktnaOsoba KontaktnaOsoba { get; set; }
    public CenovaPonuka? PoslednaCenovaPonuka { get; private set; }
    public ObjednavkaFaza Faza { get; private set; } = ObjednavkaFaza.Nacenenie;
    public bool Zrusene { get; private set; } = false;
    public bool Zaplatene { get; private set; } = false;
    public DateTime? OcakavanyDatumDorucenia { get; private set; }
    public DateTime? NaplanovanyDatumVyroby { get; private set; }
    public string? Poznamka { get; private set; }

    private readonly List<CenovaPonuka> _cenovePonuky = new();
    public IEnumerable<CenovaPonuka> CenovePonuky => _cenovePonuky.AsReadOnly();

    public void ZrusObjednavku()
    {
        if (Zrusene)
            throw new DomainValidationException("Objednávka je už zrušená.");
        if (Faza == ObjednavkaFaza.Vybavene)
            throw new DomainValidationException("Hotovú objednávku nemožno zrušiť.");
        Zrusene = true;
    }

    public void OznacAkoZaplatene()
    {
        if (Zaplatene)
            throw new DomainValidationException("Objednávka je už zaplatená.");
        Zaplatene = true;
        if (Faza == ObjednavkaFaza.PlatbaCaka)
            SetFaza(ObjednavkaFaza.Vybavene);
    }

    public void SetOcakavanyDatumDorucenia(DateTime? datum)
    {
        OcakavanyDatumDorucenia = datum;
    }

    public void SetNaplanovanyDatumVyroby(DateTime? datum)
    {
        if (Faza != ObjednavkaFaza.VyrobaNeriesene && Faza != ObjednavkaFaza.VyrobaCaka)
            throw new DomainValidationException("Dátum výroby sa dá nastaviť iba vo fázach Vyroba.");
        NaplanovanyDatumVyroby = datum;
        if (datum.HasValue && Faza == ObjednavkaFaza.VyrobaNeriesene)
            SetFaza(ObjednavkaFaza.VyrobaCaka);
    }

    public void SetPoznamka(string? poznamka)
    {
        Poznamka = poznamka;
    }

    public void AddCenovaPonuka(CenovaPonuka cenovaPonuka)
    {
        if (Faza != ObjednavkaFaza.Nacenenie)
            throw new DomainValidationException("Cenovú ponuku možno pridať iba vo fáze Nacenenie.");
        if (PoslednaCenovaPonuka != null && PoslednaCenovaPonuka.Stav != StavCenovejPonuky.Zrusene)
            throw new DomainValidationException("Nemôžete pridať novú cenovú ponuku, pokiaľ posledná cenová ponuka nie je zrušená.");
        _cenovePonuky.Add(cenovaPonuka);
        PoslednaCenovaPonuka = cenovaPonuka;
    }

    public void PoslanaCenovaPonuka()
    {
        if (PoslednaCenovaPonuka == null)
            throw new DomainValidationException("Žiadna cenová ponuka na označenie ako poslaná.");
        if (PoslednaCenovaPonuka.Stav != StavCenovejPonuky.Neriesene)
            throw new DomainValidationException("Cenová ponuka musí byť v stave Neriesene, aby sa dala označiť ako poslaná.");
        PoslednaCenovaPonuka.SetStav(StavCenovejPonuky.Poslane);
        Faza = ObjednavkaFaza.NacenenieCaka;
    }

    public void ZrusCenovuPonuku()
    {
        if (PoslednaCenovaPonuka == null)
            throw new DomainValidationException("Žiadna cenová ponuka na zrušenie.");
        if (PoslednaCenovaPonuka.Stav == StavCenovejPonuky.Schvalene)
            throw new DomainValidationException("Schválenú cenovú ponuku nemožno zrušiť. Musíte vytvoriť novú objednávku a túto zrušiť.");
        if (Faza != ObjednavkaFaza.Nacenenie && Faza != ObjednavkaFaza.NacenenieCaka)
            throw new DomainValidationException("Cenovú ponuku možno zrušiť iba vo fázach Nacenenie alebo NacenenieCaka.");
        PoslednaCenovaPonuka = null;
        Faza = ObjednavkaFaza.Nacenenie;
    }

    public void SchvalitCenovuPonuku()
    {
        if (PoslednaCenovaPonuka == null)
            throw new DomainValidationException("Žiadna cenová ponuka na schválenie.");
        if (PoslednaCenovaPonuka.Stav != StavCenovejPonuky.Poslane)
            throw new DomainValidationException("Cenová ponuka musí byť v stave Poslane, aby sa dala schváliť.");
        PoslednaCenovaPonuka.SetStav(StavCenovejPonuky.Schvalene);
        Faza = ObjednavkaFaza.VyrobaNeriesene;
    }

    public void SetFaza(ObjednavkaFaza novaFaza)
    {
        if (Faza == novaFaza)
            throw new DomainValidationException("Nemôžete nastaviť rovnakú fázu, v ktorej sa objednávka už nachádza.");

        if (Zrusene)
            throw new DomainValidationException("Zrušená objednávka nemôže meniť fázu.");

        if (novaFaza == ObjednavkaFaza.PlatbaCaka && Zaplatene)
            throw new DomainValidationException("Nemôžete nastaviť fázu 'PlatbaCaka', keď je objednávka už zaplatená.");

        if (novaFaza == ObjednavkaFaza.Nacenenie || novaFaza == ObjednavkaFaza.NacenenieCaka)
            throw new DomainValidationException("Fázy 'Nacenenie' a 'NacenenieCaka' sa nedajú nastaviť priamo.");

        // Validate allowed phase transitions
        switch (Faza)
        {
            case ObjednavkaFaza.VyrobaNeriesene:
                if (novaFaza != ObjednavkaFaza.VyrobaNemozna && novaFaza != ObjednavkaFaza.VyrobaCaka)
                    throw new DomainValidationException("Z fázy 'VyrobaNeriesene' môžete prejsť iba do fázy 'VyrobaNemozna' alebo 'VyrobaCaka'.");
                break;
            /// Keď výrobný manažér určí, že výroba je opäť možná po úprave
            /// očakávaného dátumu dodania alebo vyriešení technických problémov. Detaily je možné pridať do poznámky.
            case ObjednavkaFaza.VyrobaNemozna:
                if (novaFaza != ObjednavkaFaza.VyrobaNeriesene)
                    throw new DomainValidationException("Z fázy 'VyrobaNemozna' môžete prejsť iba do fázy 'VyrobaNeriesene'.");
                break;
            case ObjednavkaFaza.VyrobaCaka:
                if (novaFaza != ObjednavkaFaza.OdoslanieCaka && novaFaza != ObjednavkaFaza.VyrobaNemozna)
                    throw new DomainValidationException("Z fázy 'VyrobaCaka' môžete prejsť iba do fázy 'OdoslanieCaka' alebo 'VyrobaNemozna'.");
                break;
            case ObjednavkaFaza.OdoslanieCaka:
                if (novaFaza != ObjednavkaFaza.PlatbaCaka && !(novaFaza == ObjednavkaFaza.Vybavene && Zaplatene))
                    throw new DomainValidationException("Z fázy 'OdoslanieCaka' môžete prejsť iba do fázy 'PlatbaCaka' alebo 'Vybavene' (ak je objednávka zaplatená).");
                break;
            case ObjednavkaFaza.PlatbaCaka:
                if (novaFaza != ObjednavkaFaza.Vybavene)
                    throw new DomainValidationException("Z fázy 'PlatbaCaka' môžete prejsť iba do fázy 'Vybavene'.");
                break;
            default:
                throw new DomainValidationException("Neplatná fáza.");
        }

        Faza = novaFaza;
    }
} 
