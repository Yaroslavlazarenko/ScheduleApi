using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedule");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasIndex(s => new { s.ApplicationDayOfWeekId, s.PairId, s.GroupId, s.IsEvenWeek })
            .IsUnique();
        
        builder.HasOne(s => s.ApplicationDayOfWeek)
            .WithMany(d => d.Schedules)
            .HasForeignKey(s => s.ApplicationDayOfWeekId);

        builder.HasOne(s => s.Pair)
            .WithMany(p => p.Schedules)
            .HasForeignKey(s => s.PairId);

        builder.HasOne(s => s.Group)
            .WithMany(g => g.Schedules)
            .HasForeignKey(s => s.GroupId);

        builder.HasOne(s => s.Teacher)
            .WithMany(t => t.Schedules)
            .HasForeignKey(s => s.TeacherId);

        builder.HasOne(s => s.Subject)
            .WithMany(sub => sub.Schedules)
            .HasForeignKey(s => s.SubjectId);

        builder.HasOne(s => s.Semester)
            .WithMany(sem => sem.Schedules)
            .HasForeignKey(s => s.SemesterId);
    }
}