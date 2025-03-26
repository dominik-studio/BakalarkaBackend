using CRMBackend.Domain.Common;
using CRMBackend.Domain.Exceptions;
using CRMBackend.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace CRMBackend.Domain.AggregateRoots;

public class Tovar : BaseAuditableEntity, IAggregateRoot
{
    public required string InterneId { get; set; }
    public required string Nazov { get; set; }
    public string? ObrazokURL { get; private set; }
    public required KategorieProduktov Kategoria { get; set; }
    public string? Ean { get; private set; }
    public required decimal Cena
    {
        get => _cena;
        set
        {
            if (value < 0)
                throw new DomainValidationException("Cena nemôže byť záporná");
            _cena = value;
        }
    }
    private decimal _cena;
    public required Dodavatel Dodavatel { get; set; }

    private readonly List<VariantTovar> _varianty = new();
    public IEnumerable<VariantTovar> Varianty => _varianty.AsReadOnly();

    public void SetObrazok(string? obrazok)
    {
        ObrazokURL = obrazok;
    }

    public void SetEan(string? ean)
    {
        Ean = ean;
    }

    public void AddVariant(VariantTovar variant)
    {
        if (_varianty.Any(v => v.Farba == variant.Farba && Equals(v.Velkost, variant.Velkost)))
            throw new DomainValidationException("Duplicitná varianta");
        _varianty.Add(variant);
    }

    public void RemoveVariant(int variantId)
    {
        var variant = _varianty.FirstOrDefault(v => v.Id == variantId);
        if (variant == null)
            throw new DomainValidationException("Varianta nebola nájdená");
        _varianty.Remove(variant);
    }
} 
