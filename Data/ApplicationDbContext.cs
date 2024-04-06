using Microsoft.EntityFrameworkCore;
using ClassScheduling_WebApp.Models;

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

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Keys
            modelBuilder.Entity<ScheduleModel>().HasKey(s => new { s.IdCalendar, s.IdCourse, s.IdClassroom });
            modelBuilder.Entity<TechClassModel>().HasKey(tc => new { tc.IdCourse, tc.IdTechnology });
            modelBuilder.Entity<TechRoomModel>().HasKey(tr => new { tr.IdRoom, tr.IdTechnology });

            // Foreign Key Relationships
            modelBuilder.Entity<CourseModel>()
                .HasOne(c => c.Professor)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.IdProfessor);

            modelBuilder.Entity<CourseModel>()
                .HasOne(c => c.Programs)
                .WithMany(p => p.Courses)
                .HasForeignKey(c => c.IdProgram);

            modelBuilder.Entity<TechClassModel>()
                .HasOne(tc => tc.Course)
                .WithMany(c => c.TechClasses)
                .HasForeignKey(tc => tc.IdCourse);

            modelBuilder.Entity<TechClassModel>()
                .HasOne(tc => tc.Technology)
                .WithMany(t => t.TechClasses)
                .HasForeignKey(tc => tc.IdTechnology);

            modelBuilder.Entity<TechRoomModel>()
                .HasOne(tr => tr.Room)
                .WithMany()
                .HasForeignKey(tr => tr.IdRoom);

            modelBuilder.Entity<TechRoomModel>()
                .HasOne(tr => tr.Technology)
                .WithMany()
                .HasForeignKey(tr => tr.IdTechnology);

            modelBuilder.Entity<ScheduleModel>()
                .HasOne(s => s.Calendar)
                .WithMany()
                .HasForeignKey(s => s.IdCalendar);

            modelBuilder.Entity<ScheduleModel>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Schedules)
                .HasForeignKey(s => s.IdCourse);

            modelBuilder.Entity<ScheduleModel>()
                .HasOne(s => s.Classroom)
                .WithMany()
                .HasForeignKey(s => s.IdClassroom);
        }
    }
}
