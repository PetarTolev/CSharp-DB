using System.Globalization;
using Newtonsoft.Json;
using System.Linq;
using TeisterMask.Data.Models.Enums;

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
                var project = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = DateTime.ParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DueDate = string.IsNullOrEmpty(projectDto.DueDate) ? (DateTime?)null : DateTime.ParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                };

                if (!IsValid(project))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var taskDto in projectDto.Tasks)
                {
                    var task = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ExecutionType = Enum.Parse<ExecutionType>(taskDto.ExecutionType),
                        LabelType = Enum.Parse<LabelType>(taskDto.LabelType)
                    };

                    if (!IsValid(task) || task.OpenDate < project.OpenDate || task.DueDate > project.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    project.Tasks.Add(task);
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