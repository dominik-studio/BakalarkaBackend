using Plainquire.Filter.Abstractions;

namespace CRMBackend.Domain.AggregateRoots.KategorieProduktovAggregate;

[EntityFilter(Prefix = "")]
public class KategorieProduktov : BaseAuditableEntity, IAggregateRoot
{
    public required string Nazov { get; set; }
} 
