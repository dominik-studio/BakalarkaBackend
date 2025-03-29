using CRMBackend.Domain.Common;

namespace CRMBackend.Domain.AggregateRoots;

public class KontaktnaOsoba : BaseEntity
{
    public int FirmaId { get; private set; }
    public Firma Firma { get; set; } = null!;
    public required string Meno { get; set; }
    public required string Priezvisko { get; set; }
    public required string Telefon { get; set; }
    public required string Email { get; set; }
} 
