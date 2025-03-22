using CRMBackend.Domain.AggregateRoots;
using CRMBackend.Domain.Common;
using CRMBackend.Domain.Exceptions;
using CRMBackend.Domain.ValueObjects;

namespace CRMBackend.Domain.Entities;

public class Firma : BaseAuditableEntity, IAggregateRoot
{
    public required string Nazov { get; set; }
    public required string ICO { get; set; }
    public required Adresa Adresa { get; set; }
    public string? IcDph { get; private set; }
    public decimal SkoreSpolahlivosti { get; private set; } = 0.75m;
    public decimal HodnotaObjednavok { get; private set; }
    private readonly List<KontaktnaOsoba> _kontaktneOsoby = new();
    public IEnumerable<KontaktnaOsoba> KontaktneOsoby => _kontaktneOsoby.AsReadOnly();

    public void SetIcDph(string? icDph)
    {
        IcDph = icDph;
    }

    public void UpdateSkoreSpolahlivosti(decimal newSkore)
    {
        if (newSkore < 0 || newSkore > 1)
            throw new DomainValidationException("Skore spolahlivosti must be between 0 and 1");
        SkoreSpolahlivosti = newSkore;
    }

    public void UpdateHodnotaObjednavok(decimal newHodnota)
    {
        if (newHodnota < 0)
            throw new DomainValidationException("Hodnota objednavok cannot be negative");
        HodnotaObjednavok = newHodnota;
    }

    public void AddKontaktnaOsoba(KontaktnaOsoba osoba)
    {
        if (_kontaktneOsoby.Any(o => o.Email == osoba.Email))
            throw new DomainValidationException("Duplicate email not allowed");

        _kontaktneOsoby.Add(osoba);
    }

    public void RemoveKontaktnaOsoba(int osobaId)
    {
        var osoba = _kontaktneOsoby.FirstOrDefault(o => o.Id == osobaId);
        if (osoba != null)
        {
            _kontaktneOsoby.Remove(osoba);
        }
    }
} 
