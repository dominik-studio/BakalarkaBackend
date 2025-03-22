using CRMBackend.Domain.Common;
using CRMBackend.Domain.Exceptions;
using CRMBackend.Domain.ValueObjects;

namespace CRMBackend.Domain.AggregateRoots;

public class VariantTovar : BaseEntity
{
    public required Tovar Tovar { get; set; }
    public string? Farba { get; private set; }
    public Velkost? Velkost { get; private set; }
    public required decimal Cena
    {
        get => _cena;
        set
        {
            if (value < 0)
                throw new DomainValidationException("Cena cannot be negative");
            _cena = value;
        }
    }
    private decimal _cena;
    public string? ObrazokURL { get; private set; }

    public void SetFarbaVelkost(string? farba, Velkost? velkost)
    {
        if (string.IsNullOrEmpty(farba) && velkost == null)
            throw new DomainValidationException("At least one of Farba or Velkost must be set");

        if (!string.IsNullOrEmpty(farba) && !IsValidHexColor(farba))
            throw new DomainValidationException("Farba must be a valid hex color code");

        Farba = farba;
        Velkost = velkost;
    }

    public void SetCena(decimal cena)
    {
        if (cena < 0)
            throw new DomainValidationException("Cena cannot be negative");
        Cena = cena;
    }

    public void SetObrazok(string? obrazok)
    {
        ObrazokURL = obrazok;
    }

    private static bool IsValidHexColor(string color)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(color, @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
    }
} 
