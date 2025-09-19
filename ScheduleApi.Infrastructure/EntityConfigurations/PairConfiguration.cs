using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class PairConfiguration : IEntityTypeConfiguration<Pair>
{
    public void Configure(EntityTypeBuilder<Pair> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
    }
}