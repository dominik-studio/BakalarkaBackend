using AutoMapper;
using CRMBackend.Domain.AggregateRoots.FirmaAggregate;

namespace CRMBackend.Application.ObjednavkaAggregate.Queries.Objednavky.ListObjednavky;

public record KontaktnaOsobaDTO
{
    public int Id { get; init; }
    public required string Meno { get; init; }
    public required string Priezvisko { get; init; }
    public string? Telefon { get; init; }
    public string? Email { get; init; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<KontaktnaOsoba, KontaktnaOsobaDTO>();
        }
    }
} 
