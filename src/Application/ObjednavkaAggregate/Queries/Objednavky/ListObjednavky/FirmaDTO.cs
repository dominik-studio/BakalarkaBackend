using AutoMapper;
using CRMBackend.Domain.AggregateRoots.FirmaAggregate;

namespace CRMBackend.Application.ObjednavkaAggregate.Queries.Objednavky.ListObjednavky;

public record FirmaDTO
{
    public int Id { get; init; }
    public required string Nazov { get; init; }
    public required string ICO { get; init; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Firma, FirmaDTO>();
        }
    }
} 
