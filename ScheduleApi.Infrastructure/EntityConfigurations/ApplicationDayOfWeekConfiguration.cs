using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class ApplicationDayOfWeekConfiguration : IEntityTypeConfiguration<ApplicationDayOfWeek>
{
    public void Configure(EntityTypeBuilder<ApplicationDayOfWeek> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).ValueGeneratedOnAdd();
    }
}