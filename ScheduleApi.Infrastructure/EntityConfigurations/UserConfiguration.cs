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

        builder.HasOne(u => u.Group)
            .WithMany(g => g.Users)
            .HasForeignKey(u => u.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Region)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RegionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}