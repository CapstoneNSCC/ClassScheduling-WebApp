using Microsoft.EntityFrameworkCore;
using ClassScheduling_WebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ClassScheduling_WebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CalendarModel> Calendars { get; set; }
        public DbSet<ClassroomModel> Classrooms { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<ProgramModel> Programs { get; set; }
        public DbSet<ScheduleModel> Schedules { get; set; }
        public DbSet<TechClassModel> TechClasses { get; set; }
        public DbSet<TechnologyModel> Technologies { get; set; }
        public DbSet<TechRoomModel> TechRooms { get; set; }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite key for ScheduleModel
            modelBuilder.Entity<ScheduleModel>().HasKey(s => new { s.IdCalendar, s.IdCourse, s.IdClassroom });
            modelBuilder.Entity<TechClassModel>().HasKey(tc => new { tc.IdCourse, tc.IdTechnology });
            modelBuilder.Entity<TechRoomModel>().HasKey(tr => new { tr.IdRoom, tr.IdTechnology });

        }
    }
}
