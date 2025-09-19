using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class SocialMediaTypeConfiguration : IEntityTypeConfiguration<SocialMediaType>
{
    public void Configure(EntityTypeBuilder<SocialMediaType> builder)
    {
        builder.HasKey(smt => smt.Id);
        builder.Property(smt => smt.Id).ValueGeneratedOnAdd();
    }
}