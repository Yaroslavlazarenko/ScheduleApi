using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class ScheduleOverrideConfiguration : IEntityTypeConfiguration<ScheduleOverride>
{
    public void Configure(EntityTypeBuilder<ScheduleOverride> builder)
    {
        builder.HasKey(so => so.Id);
        builder.Property(so => so.Id).ValueGeneratedOnAdd();
        builder.Property(so => so.OverrideDate).IsRequired();
        builder.Property(so => so.OverrideTypeId).IsRequired();

        builder.HasOne(so => so.OverrideType)
            .WithMany(ot => ot.ScheduleOverrides)
            .HasForeignKey(so => so.OverrideTypeId);

        builder.HasOne(so => so.SubstituteDayOfWeek)
            .WithMany(d => d.ScheduleOverrides)
            .HasForeignKey(so => so.SubstituteDayOfWeekId);

        builder.HasOne(so => so.Group)
            .WithMany(g => g.ScheduleOverrides)
            .HasForeignKey(so => so.GroupId);
    }
}