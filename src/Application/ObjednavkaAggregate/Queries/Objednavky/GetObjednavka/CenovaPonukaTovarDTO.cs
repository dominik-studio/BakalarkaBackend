using AutoMapper;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;
using CRMBackend.Domain.ValueObjects;

namespace CRMBackend.Application.ObjednavkaAggregate.Queries.Objednavky.GetObjednavka;

public record CenovaPonukaTovarDTO
{
    public required int Id { get; init; }
    public required string NazovTovaru { get; init; }
    public required int Mnozstvo { get; init; }
    public required decimal PovodnaCena { get; init; }
    public Velkost? Velkost { get; init; }
    public String? FarbaHex { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CenovaPonukaTovar, CenovaPonukaTovarDTO>()
                .ForMember(dest => dest.NazovTovaru, opt => opt.MapFrom(src => 
                    src.Tovar != null 
                    ? src.Tovar.Nazov 
                    : src.VariantTovar != null 
                        ? src.VariantTovar.Tovar.Nazov
                        : string.Empty))
                .ForMember(dest => dest.Velkost, opt => opt.MapFrom(src => 
                    src.VariantTovar != null ? src.VariantTovar.Velkost : null))
                .ForMember(dest => dest.FarbaHex, opt => opt.MapFrom(src => 
                    src.VariantTovar != null ? src.VariantTovar.FarbaHex : null));
        }
    }
}
