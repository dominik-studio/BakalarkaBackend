using Plainquire.Filter.Abstractions;
using PromobayBackend.Domain.Common;
using PromobayBackend.Domain.Exceptions;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;

namespace PromobayBackend.Domain.AggregateRoots.BrandAggregate;

[EntityFilter(Prefix = "")]
public class Brand : BaseAuditableEntity, IAggregateRoot
{
    public required string Name { get; set; }
    public string? Description { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? WebsiteUrl { get; private set; }

    private readonly List<Campaign> _campaigns = new();
    public IEnumerable<Campaign> Campaigns => _campaigns.AsReadOnly();

    public void SetDescription(string? description)
    {
        Description = description;
    }

    public void SetLogoUrl(string? logoUrl)
    {
        if (!string.IsNullOrEmpty(logoUrl) && !Uri.IsWellFormedUriString(logoUrl, UriKind.Absolute))
            throw new DomainValidationException("Logo URL must be a valid absolute URI.");
        LogoUrl = logoUrl;
    }

    public void SetWebsiteUrl(string? websiteUrl)
    {
        if (!string.IsNullOrEmpty(websiteUrl) && !Uri.IsWellFormedUriString(websiteUrl, UriKind.Absolute))
            throw new DomainValidationException("Website URL must be a valid absolute URI.");
        WebsiteUrl = websiteUrl;
    }
} 
