using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class BroadcastConfiguration : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.Property(b => b.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now() at time zone 'utc'"); 
        
        builder.Property(b => b.ScheduledAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(b => b.SentAt)
            .IsRequired(false)
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(b => new { b.SentAt, b.ScheduledAt });
    }
}