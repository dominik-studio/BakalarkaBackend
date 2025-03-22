using CRMBackend.Domain.Common;
using CRMBackend.Domain.Exceptions;

namespace CRMBackend.Domain.AggregateRoots;

public class CenovaPonukaTovar : BaseEntity
{
    public int CenovaPonukaId { get; set; }
    public CenovaPonuka CenovaPonuka { get; set; } = null!;
    public Tovar? Tovar { get; private set; }
    public VariantTovar? VariantTovar { get; private set; }
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

    public void SetTovar(Tovar? tovar)
    {
        if (tovar != null && VariantTovar != null)
            throw new DomainValidationException("Tovar a variant nemôžu byť nastavené súčasne.");
        Tovar = tovar;
    }

    public void SetVariantTovar(VariantTovar? variantTovar)
    {
        if (variantTovar != null && Tovar != null)
            throw new DomainValidationException("Tovar a variant nemôžu byť nastavené súčasne.");
        VariantTovar = variantTovar;
    }

    public void SetPovodnaCena(decimal cena)
    {
        if (cena < 0)
            throw new DomainValidationException("Pôvodná cena nemôže byť záporná.");
        PovodnaCena = cena;
    }
}