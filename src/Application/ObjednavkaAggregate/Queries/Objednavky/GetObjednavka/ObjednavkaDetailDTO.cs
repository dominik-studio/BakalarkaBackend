using AutoMapper;
using CRMBackend.Application.Common.Mappings;
using CRMBackend.Application.Common.Models;
using CRMBackend.Application.FirmaAggregate.Queries.ListFirmy; // Assuming FirmaDTO location
using CRMBackend.Application.FirmaAggregate.Queries.GetFirma; // Assuming KontaktnaOsobaDTO location
using CRMBackend.Domain.AggregateRoots.FirmaAggregate;
using CRMBackend.Domain.AggregateRoots.ObjednavkaAggregate;
using CRMBackend.Domain.Enums;

namespace CRMBackend.Application.ObjednavkaAggregate.Queries.Objednavky.GetObjednavka;

public record ObjednavkaDetailDTO : BaseAuditableDto
{
    public required FirmaDTO Firma { get; init; }
    public required KontaktnaOsobaDTO KontaktnaOsoba { get; init; }
    public ObjednavkaFaza Faza { get; init; }
    public string? Poznamka { get; init; }
    public ChybaKlienta? ChybaKlienta { get; init; }
    public DateTime? OcakavanyDatumDorucenia { get; init; }
    public DateTime? NaplanovanyDatumVyroby { get; init; }
    public bool Zrusene { get; init; }
    public bool Zaplatene { get; init; }
    public int? PoslednaCenovaPonukaId { get; init; }
    public required IEnumerable<CenovaPonukaDTO> CenovePonuky { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Objednavka, ObjednavkaDetailDTO>();
        }
    }
} 
