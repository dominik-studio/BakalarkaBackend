using CRMBackend.Domain.Common;
using CRMBackend.Domain.ValueObjects;

namespace CRMBackend.Domain.AggregateRoots;

public class Dodavatel : BaseAuditableEntity, IAggregateRoot
{
    public required string NazovFirmy { get; set; }
    public required string Email { get; set; }
    public required string Telefon { get; set; }
    public Adresa? Adresa { get; private set; }
    public string? Poznamka { get; private set; }
    public bool Aktivny { get; set; } = true;

    public void SetAdresa(Adresa? adresa)
    {
        Adresa = adresa;
    }

    public void SetPoznamka(string? poznamka)
    {
        Poznamka = poznamka;
    }
} 
