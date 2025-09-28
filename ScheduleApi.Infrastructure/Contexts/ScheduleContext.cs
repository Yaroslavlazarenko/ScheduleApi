using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Core.Entities;

namespace ScheduleBotApi.Infrastructure.Contexts;

public class ScheduleContext : DbContext
{
    public ScheduleContext(DbContextOptions<ScheduleContext> options)
        : base(options)
    {
    }
    
    public DbSet<ApplicationDayOfWeek> ApplicationDaysOfWeek { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupSubject> GroupSubjects { get; set; }
    public DbSet<InfoType> InfoTypes { get; set; }
    public DbSet<OverrideType> OverrideTypes { get; set; }
    public DbSet<Pair> Pairs { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ScheduleOverride> ScheduleOverrides { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<SocialMediaType> SocialMediaTypes { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectName> SubjectNames { get; set; }
    public DbSet<SubjectInfo> SubjectInfos { get; set; }
    public DbSet<SubjectType> SubjectTypes { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<TeacherInfo> TeacherInfos { get; set; }
    public DbSet<TeacherSubject> TeacherSubjects { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}