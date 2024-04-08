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
        public DbSet<TechClassModel> TechClasses { get; set; }
        public DbSet<TechnologyModel> Technologies { get; set; }
        public DbSet<TechRoomModel> TechRooms { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public DbSet<EventModel> TblEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Keys
            modelBuilder.Entity<TechClassModel>().HasKey(tc => new { tc.IdCourse, tc.IdTechnology });
            modelBuilder.Entity<TechRoomModel>().HasKey(tr => new { tr.IdClassroom, tr.IdTechnology });

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
                .HasOne(tr => tr.Classroom)
                .WithMany(t => t.TechRooms)
                .HasForeignKey(tr => tr.IdClassroom);

            modelBuilder.Entity<TechRoomModel>()
                .HasOne(tr => tr.Technology)
                .WithMany(t => t.TechRooms)
                .HasForeignKey(tr => tr.IdTechnology);
        }
    }
}