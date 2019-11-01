namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enums;
    using System;

    internal static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData
            (
                new Student
                {
                    StudentId = 1,
                    Name = "Michael Henderson",
                    RegisteredOn = DateTime.Now
                },

                new Student
                {
                    StudentId = 2,
                    Name = "Kate Milner",
                    RegisteredOn = DateTime.Now
                }
            );

            modelBuilder.Entity<Course>().HasData
            (
                new Course
                {
                    CourseId = 1,
                    Name = "MSSQL Server",
                    Price = 200m
                },

                new Course
                {
                    CourseId = 2,
                    Name = "Entity Framework Core",
                    Price = 280m
                }
            );

            modelBuilder.Entity<Resource>().HasData
            (
                new Resource
                {
                    ResourceId = 1,
                    CourseId = 1,
                    Name = "Course resources",
                    ResourceType = ResourceType.Document
                },

                new Resource
                {
                    ResourceId = 2,
                    CourseId = 2,
                    Name = "Course introduction",
                    ResourceType = ResourceType.Presentation
                }
            );

            modelBuilder.Entity<Homework>().HasData
            (
                new Homework
                {
                    HomeworkId = 1,
                    StudentId = 2,
                    CourseId = 1,
                    ContentType = ContentType.Zip,
                    SubmissionTime = DateTime.Now
                }
            );
        }
    }
}