namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureStudentEntity(modelBuilder);

            ConfigureCourseEntity(modelBuilder);

            ConfigureResourceEntity(modelBuilder);

            ConfigureHomeworkEntity(modelBuilder);

            ConfigureStudentCourseEntity(modelBuilder);

            modelBuilder.Seed();
        }

        private void ConfigureStudentCourseEntity(ModelBuilder mb)
        {
            mb.Entity<StudentCourse>()
                .HasKey(sc => new {sc.StudentId, sc.CourseId});
        }

        private void ConfigureHomeworkEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Homework>(homework =>
            {
                homework
                    .HasKey(h => h.HomeworkId);

                homework
                    .Property(h => h.Content)
                    .IsUnicode(false);
            });
        }

        private void ConfigureResourceEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>(resource =>
            {
                resource
                    .HasKey(r => r.ResourceId);

                resource
                    .Property(r => r.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                resource
                    .Property(r => r.Url)
                    .IsUnicode(false);
            });
        }

        private void ConfigureCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(course =>
            {
                course
                    .HasKey(c => c.CourseId);

                course
                     .HasMany(c => c.StudentsEnrolled)
                     .WithOne(s => s.Course);

                course
                    .HasMany(c => c.Resources)
                    .WithOne(r => r.Course);

                course
                    .HasMany(c => c.HomeworkSubmissions)
                    .WithOne(h => h.Course);

                course
                    .Property(c => c.Name)
                    .HasMaxLength(80)
                    .IsUnicode()
                    .IsRequired();

                course
                    .Property(c => c.Description)
                    .IsRequired(false)
                    .IsUnicode();
            });
        }

        private void ConfigureStudentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(student =>
            {
                student
                    .HasKey(s => s.StudentId);

                student
                    .HasMany(s => s.CourseEnrollments)
                    .WithOne(ce => ce.Student);

                student
                    .HasMany(s => s.HomeworkSubmissions)
                    .WithOne(h => h.Student);

                student
                    .Property(s => s.Name)
                    .HasMaxLength(100)
                    .IsUnicode()
                    .IsRequired();

                student
                    .Property(s => s.PhoneNumber)
                    .HasColumnType("CHAR(10)")
                    .IsUnicode(false)
                    .IsRequired(false);
            });
        }
    }
}
