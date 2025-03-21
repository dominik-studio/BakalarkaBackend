using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;

namespace PromobayBackend.Domain.AggregateRoots.CampaignAggregate;

public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired();
        
        builder.HasOne(c => c.Brand)
            .WithMany(b => b.Campaigns)
            .HasForeignKey(c => c.BrandId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
