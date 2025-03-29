using CRMBackend.Domain.Common;
using CRMBackend.Domain.Exceptions;

namespace CRMBackend.Domain.AggregateRoots;

public class CenovaPonukaTovar : BaseEntity
{
    public int CenovaPonukaId { get; private set; }
    public CenovaPonuka CenovaPonuka { get; set; } = null!;
    public Tovar? Tovar { get; private set; }
    public int? TovarId { get; private set; }
    public VariantTovar? VariantTovar { get; private set; }
    public int? VariantTovarId { get; private set; }
    public required int Mnozstvo
    {
        get => _mnozstvo;
        set
        {
            if (value <= 0)
                throw new DomainValidationException("Množstvo musí byť väčšie ako 0.");
            _mnozstvo = value;
        }
    }
    private int _mnozstvo;
    public decimal PovodnaCena { get; private set; }

    // Private parameterless constructor for EF Core
    private CenovaPonukaTovar() { }

    public CenovaPonukaTovar(Tovar? tovar = null, VariantTovar? variantTovar = null)
    {
        if (tovar == null && variantTovar == null)
            throw new DomainValidationException("Musí byť nastavený tovar alebo variant.");
        if (tovar != null && variantTovar != null)
            throw new DomainValidationException("Tovar a variant nemôžu byť nastavené súčasne.");

        Tovar = tovar;
        TovarId = tovar?.Id;
        VariantTovar = variantTovar;
        VariantTovarId = variantTovar?.Id;
    }

    public void SetTovar(Tovar? tovar)
    {
        if (tovar != null && VariantTovar != null)
        {
            VariantTovar = null;
            VariantTovarId = null;
        }
        Tovar = tovar;
        TovarId = tovar?.Id;
    }

    public void SetVariantTovar(VariantTovar? variantTovar)
    {
        if (variantTovar != null && Tovar != null)
        {
            Tovar = null;
            TovarId = null;
        }
        VariantTovar = variantTovar;
        VariantTovarId = variantTovar?.Id;
    }

    public void SetPovodnaCena(decimal cena)
    {
        if (cena < 0)
            throw new DomainValidationException("Pôvodná cena nemôže byť záporná.");
        PovodnaCena = cena;
    }
}
