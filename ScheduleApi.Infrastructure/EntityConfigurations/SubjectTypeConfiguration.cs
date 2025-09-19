using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class SubjectTypeConfiguration : IEntityTypeConfiguration<SubjectType>
{
    public void Configure(EntityTypeBuilder<SubjectType> builder)
    {
        builder.HasKey(st => st.Id);
        builder.Property(st => st.Id).ValueGeneratedOnAdd();
    }
}