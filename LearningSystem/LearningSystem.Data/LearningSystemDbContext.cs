namespace LearningSystem.Data
{
    using LearningSystem.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class LearningSystemDbContext : IdentityDbContext<User>
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public LearningSystemDbContext(DbContextOptions<LearningSystemDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<StudentCourse>()
                .HasKey(sc => new { sc.CourseId, sc.StudentId });

            // One Student have many Courses
            builder
                .Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(sc => sc.StudentId);

            // One Course have many Students
            builder
                .Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(sc => sc.CourseId);

            builder
                .Entity<Course>()
                .HasOne(c => c.Trainer)
                .WithMany(u => u.Trainings)
                .HasForeignKey(c => c.TrainerId);

            builder
                .Entity<Article>()
                .HasOne(a => a.Author)
                .WithMany(u => u.Articles)
                .HasForeignKey(a => a.AuthorId);

            builder
                .Entity<Exam>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.CourseId);

            builder
                .Entity<Exam>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Exams)
                .HasForeignKey(e => e.StudentId);


            base.OnModelCreating(builder);
        }
    }
}
