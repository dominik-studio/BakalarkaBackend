using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;
using PromobayBackend.Domain.Enums;

namespace PromobayBackend.Application.CampaignAggregate.Queries.GetCampaigns;
using AutoMapper;

public class CampaignDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CampaignStart { get; set; }
    public DateTime? CampaignEnd { get; set; }
    public CampaignStatus Status { get; set; }
    public int BrandId { get; set; }
    public string? BrandName { get; set; }
    public string? BrandLogoUrl { get; set; }
    public string? BrandWebsiteUrl { get; set; }
    public string? BrandDescription { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Campaign, CampaignDto>();
        }
    }
} 
