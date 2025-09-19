using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class SubjectInfoConfiguration : IEntityTypeConfiguration<SubjectInfo>
{
    public void Configure(EntityTypeBuilder<SubjectInfo> builder)
    {
        builder.HasKey(si => new { si.SubjectId, si.InfoTypeId });

        builder.HasOne(si => si.Subject)
            .WithMany(s => s.SubjectInfos)
            .HasForeignKey(si => si.SubjectId);

        builder.HasOne(si => si.InfoType)
            .WithMany(it => it.SubjectInfos)
            .HasForeignKey(si => si.InfoTypeId);
    }
}