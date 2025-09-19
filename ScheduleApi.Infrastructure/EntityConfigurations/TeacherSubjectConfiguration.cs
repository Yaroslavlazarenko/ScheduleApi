using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class TeacherSubjectConfiguration : IEntityTypeConfiguration<TeacherSubject>
{
    public void Configure(EntityTypeBuilder<TeacherSubject> builder)
    {
        builder.HasKey(ts => new { ts.TeacherId, ts.SubjectId });

        builder.HasOne(ts => ts.Teacher)
            .WithMany(t => t.TeacherSubjects)
            .HasForeignKey(ts => ts.TeacherId);

        builder.HasOne(ts => ts.Subject)
            .WithMany(s => s.TeacherSubjects)
            .HasForeignKey(ts => ts.SubjectId);

        builder.HasOne(ts => ts.SocialMediaType)
            .WithMany(smt => smt.TeacherSubjects)
            .HasForeignKey(ts => ts.SocialMediaTypesId);
    }
}