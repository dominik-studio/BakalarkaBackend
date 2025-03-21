using PromobayBackend.Domain.AggregateRoots.BrandAggregate;

namespace PromobayBackend.Application.BrandAggregate.Queries.GetBrands;

public class BrandDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? LogoUrl { get; init; }
    public string? WebsiteUrl { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Brand, BrandDto>();
        }
    }
} 