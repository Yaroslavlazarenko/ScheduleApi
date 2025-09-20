using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class InfoTypeConfiguration : IEntityTypeConfiguration<InfoType>
{
    public void Configure(EntityTypeBuilder<InfoType> builder)
    {
        builder.HasKey(it => it.Id);
        builder.Property(it => it.Id).ValueGeneratedOnAdd();
    }
}