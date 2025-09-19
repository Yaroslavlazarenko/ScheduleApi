using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class GroupSubjectConfiguration : IEntityTypeConfiguration<GroupSubject>
{
    public void Configure(EntityTypeBuilder<GroupSubject> builder)
    {
        builder.HasKey(gs => new { gs.GroupId, gs.TeacherId, gs.SubjectId });

        builder.HasOne(gs => gs.Group)
            .WithMany(g => g.GroupSubjects)
            .HasForeignKey(gs => gs.GroupId);

        builder.HasOne(gs => gs.Teacher)
            .WithMany(t => t.GroupSubjects)
            .HasForeignKey(gs => gs.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(gs => gs.Subject)
            .WithMany(s => s.GroupSubjects)
            .HasForeignKey(gs => gs.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(gs => gs.TeacherSubject)
            .WithMany(ts => ts.GroupSubjects)
            .HasForeignKey(gs => new { gs.TeacherId, gs.SubjectId });
    }
}