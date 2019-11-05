namespace P01_StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> student)
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
        }
    }
}
