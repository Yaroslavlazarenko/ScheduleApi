using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");
        
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(s => new { s.ApplicationDayOfWeekId, s.PairId, s.GroupSubjectId, s.IsEvenWeek })
            .IsUnique();
        
        builder.HasOne(s => s.ApplicationDayOfWeek)
            .WithMany(d => d.Schedules)
            .HasForeignKey(s => s.ApplicationDayOfWeekId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Pair)
            .WithMany(p => p.Schedules)
            .HasForeignKey(s => s.PairId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.GroupSubject)
            .WithMany(gs => gs.Schedules)
            .HasForeignKey(s => s.GroupSubjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}