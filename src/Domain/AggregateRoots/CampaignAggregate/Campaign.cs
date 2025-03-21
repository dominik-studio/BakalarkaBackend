using PromobayBackend.Domain.Common;
using PromobayBackend.Domain.Exceptions;
using PromobayBackend.Domain.ValueObjects;
using PromobayBackend.Domain.Enums;
using PromobayBackend.Domain.AggregateRoots.BrandAggregate;
using System.Text.Json;
using System.Linq;
using Plainquire.Filter.Abstractions;

namespace PromobayBackend.Domain.AggregateRoots.CampaignAggregate;

[EntityFilter(Prefix = "")]
public class Campaign : BaseAuditableEntity, IAggregateRoot
{
    public required string Name { get; set; }
    public string? Description { get; private set; }
    public DateTime? CampaignStart { get; private set; }
    public DateTime? CampaignEnd { get; private set; }

    public CampaignStatus Status 
    { 
        get 
        {
            if (_status == CampaignStatus.Published && Expired)
                return CampaignStatus.Expired;
            return _status;
        }
        private set => _status = value;
    }
    public bool Expired => CampaignEnd.HasValue && CampaignEnd.Value < DateTime.UtcNow;

    private CampaignStatus _status = CampaignStatus.Draft;
    public int BrandId { get; private set; }
    public required Brand Brand { get; set; }

    private string? Reward;
    public string? RewardJson
    {
        get => Reward;
        private set
        {
            ValidateRewardJson(value);
            Reward = value;
        }
    }

    public void SetDescription(string? description)
    {
        Description = description;
    }

    public void SetDates(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new DomainValidationException("Campaign end must be after start.");
        
        CampaignStart = start;
        CampaignEnd = end;
    }

    public void SetReward(string? rewardJson)
    {
        RewardJson = rewardJson;
    }

    public int GetCurrentReward(int followerCount)
    {
        if (RewardJson == null)
            return 0;

        var tiers = JsonSerializer.Deserialize<List<RewardTier>>(RewardJson);
        if (tiers == null || !tiers.Any())
            return 0;

        var tier = tiers
            .Where(t => followerCount >= t.MinFollowers)
            .OrderByDescending(t => t.MinFollowers)
            .FirstOrDefault();

        return tier?.MinFollowers ?? 0;
    }
    
    private class RewardTier
    {
        public int MinFollowers { get; private set; }
        public decimal Price { get; private set; }

        public RewardTier(int minFollowers, decimal price)
        {
            MinFollowers = minFollowers;
            Price = price;
        }
    }

    private void ValidateRewardJson(string? json)
    {
        if (string.IsNullOrEmpty(json))
            throw new DomainValidationException("Invalid JSON format.");

        var tiers = JsonSerializer.Deserialize<List<RewardTier>>(json);
        if (tiers == null || !tiers.Any())
            throw new DomainValidationException("Invalid JSON format or empty tiers.");

        if (tiers.Count == 0)
            throw new DomainValidationException("At least one reward tier is required.");

        for (int i = 1; i < tiers.Count; i++)
        {
            if (tiers[i].MinFollowers <= tiers[i - 1].MinFollowers)
            {
                throw new DomainValidationException("MinFollowers must be in ascending order.");
            }
        }
    }

    public void Publish()
    {
        if (CampaignStart == null || CampaignEnd == null || 
            Description == null || RewardJson == null)
            throw new DomainValidationException("All fields must be set before publishing.");
        
        Status = CampaignStatus.Published;
    }

    public void SetToDraft()
    {
        Status = CampaignStatus.Draft;
    }
}
