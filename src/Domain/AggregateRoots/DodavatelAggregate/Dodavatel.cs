using CRMBackend.Domain.Common;

namespace CRMBackend.Domain.AggregateRoots;

public class Dodavatel : BaseAuditableEntity, IAggregateRoot
{
    public required string NazovFirmy { get; set; }
} 
