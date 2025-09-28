using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(s => new { s.SubjectNameId, s.SubjectTypeId }).IsUnique();

        builder.HasOne(s => s.SubjectType)
            .WithMany(st => st.Subjects)
            .HasForeignKey(s => s.SubjectTypeId);
        
        builder.HasOne(s => s.SubjectName)
            .WithMany(sn => sn.Subjects)
            .HasForeignKey(s => s.SubjectNameId);
    }
}