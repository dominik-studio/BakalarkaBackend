using CRMBackend.Domain.Common;

namespace CRMBackend.Domain.AggregateRoots;

public class KategorieProduktov : BaseAuditableEntity, IAggregateRoot
{
    public required string Nazov { get; set; }
} 
