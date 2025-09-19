using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class OverrideTypeConfiguration : IEntityTypeConfiguration<OverrideType>
{
    public void Configure(EntityTypeBuilder<OverrideType> builder)
    {
        builder.HasKey(ot => ot.Id);
        builder.Property(ot => ot.Id).ValueGeneratedOnAdd();
        builder.Property(ot => ot.Name).IsRequired();
    }
}