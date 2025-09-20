using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(u => u.TelegramId).IsUnique();

        builder.HasOne(s => s.Group)
            .WithMany(g => g.Users)
            .HasForeignKey(s => s.GroupId);

        builder.HasOne(s => s.Region)
            .WithMany(tz => tz.Users)
            .HasForeignKey(s => s.RegionId);
    }
}