using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class TeacherInfoConfiguration : IEntityTypeConfiguration<TeacherInfo>
{
    public void Configure(EntityTypeBuilder<TeacherInfo> builder)
    {
        builder.HasKey(ti => new { ti.TeacherId, ti.InfoTypeId });

        builder.HasOne(ti => ti.Teacher)
            .WithMany(t => t.TeacherInfos)
            .HasForeignKey(ti => ti.TeacherId);

        builder.HasOne(ti => ti.InfoType)
            .WithMany(it => it.TeacherInfos)
            .HasForeignKey(ti => ti.InfoTypeId);
    }
}