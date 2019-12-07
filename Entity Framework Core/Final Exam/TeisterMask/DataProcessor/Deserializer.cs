using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace TeisterMask.DataProcessor
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProjectDto[]),
                new XmlRootAttribute("Projects"));

            var projectsDto = (ProjectDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validProjects = new List<Project>();
            var sb = new StringBuilder();

            foreach (var projectDto in projectsDto)
            {
                var tasks = Mapper.Map<Task[]>(projectDto.Tasks);
                var validTasks = new List<Task>();
                var project = Mapper.Map<Project>(projectDto);

                foreach (var task in tasks)
                {
                    var a = task.OpenDate.CompareTo(project.OpenDate) <= 0;
                    //var b = task.DueDate.CompareTo(project.DueDate) >= 0;
                    bool b;

                    if (project.DueDate.HasValue)
                    {
                        b = task.DueDate.CompareTo(project.DueDate.Value) >= 0;
                    }
                    else
                    {
                        b = false;
                    }

                    if (!IsValid(task) || a || b)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validTasks.Add(task);
                }

                project.Tasks = validTasks;

                if (!IsValid(project)) //|| !tasks.All(IsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validProjects.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }
	
            context.Projects.AddRange(validProjects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeesDto = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString).ToArray();

            var validEmployees = new List<Data.Models.Employee>();
            var sb =new StringBuilder();

            foreach (var employeeDto in employeesDto)
            {
                var tasksId = context.Tasks.Select(t => t.Id);

                if (!IsValid(employeeDto) || !tasksId.Any(x => employeeDto.EmployeesTasks.Contains(x)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employee = Mapper.Map<Data.Models.Employee>((object)employeeDto);

                //var employee = new Employee
                //{
                //    Username = employeeDto.Username,
                //    Email = employeeDto.Email,
                //    Phone = employeeDto.Phone,
                //    EmployeesTasks = 
                //};

                validEmployees.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username,
                    employee.EmployeesTasks.Sum(et => et.Task.Id)));
            }
            

            context.Employees.AddRange(validEmployees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}