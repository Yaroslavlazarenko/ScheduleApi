using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class GroupSubjectConfiguration : IEntityTypeConfiguration<GroupSubject>
{
    public void Configure(EntityTypeBuilder<GroupSubject> builder)
    {
        builder.HasKey(gs => gs.Id);
        builder.Property(gs => gs.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(gs => new { gs.GroupId, gs.TeacherSubjectId, gs.SemesterId }).IsUnique();
        
        builder.HasOne(gs => gs.Group)
            .WithMany(g => g.GroupSubjects)
            .HasForeignKey(gs => gs.GroupId);

        builder.HasOne(gs => gs.TeacherSubject)
            .WithMany(ts => ts.GroupSubjects)
            .HasForeignKey(gs => gs.TeacherSubjectId);
        
        builder.HasOne(gs => gs.Semester)
            .WithMany(s => s.GroupSubjects)
            .HasForeignKey(gs => gs.SemesterId);
    }
}