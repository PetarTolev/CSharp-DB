namespace P01_StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> course)
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
        }
    }
}
