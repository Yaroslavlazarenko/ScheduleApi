using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class TeacherSubjectConfiguration : IEntityTypeConfiguration<TeacherSubject>
{
    public void Configure(EntityTypeBuilder<TeacherSubject> builder)
    {
        builder.HasKey(ts => ts.Id);
        builder.Property(ts => ts.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(ts => new { ts.TeacherId, ts.SubjectId }).IsUnique();
        
        builder.HasOne(ts => ts.Teacher)
            .WithMany(t => t.TeacherSubjects)
            .HasForeignKey(ts => ts.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(ts => ts.Subject)
            .WithMany(s => s.TeacherSubjects)
            .HasForeignKey(ts => ts.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(ts => ts.SocialMediaType)
            .WithMany(smt => smt.TeacherSubjects)
            .HasForeignKey(ts => ts.SocialMediaTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}