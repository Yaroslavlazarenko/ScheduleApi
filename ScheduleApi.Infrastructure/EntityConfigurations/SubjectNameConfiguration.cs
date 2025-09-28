using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class SubjectNameConfiguration : IEntityTypeConfiguration<SubjectName>
{
    public void Configure(EntityTypeBuilder<SubjectName> builder)
    {
        builder.HasKey(sn => sn.Id);
        builder.Property(sn => sn.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(sn => sn.Abbreviation).IsUnique();
    }
}