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

        builder.Property(s => s.GroupId).IsRequired();
        builder.Property(s => s.RegionId).IsRequired();
        builder.Property(s => s.IsAdmin).IsRequired();

        builder.HasOne(s => s.Group)
            .WithMany(g => g.Users)
            .HasForeignKey(s => s.GroupId);

        builder.HasOne(s => s.Region)
            .WithMany(tz => tz.Students)
            .HasForeignKey(s => s.RegionId);
    }
}