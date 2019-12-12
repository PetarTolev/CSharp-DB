﻿using System.Globalization;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TeisterMask.DataProcessor.ExportDto;
using ProjectDto = TeisterMask.DataProcessor.ExportDto.ProjectDto;
using TaskDto = TeisterMask.DataProcessor.ExportDto.TaskDto;

namespace TeisterMask.DataProcessor
{
    using Data;
    using System;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projects = context.Projects
                .Where(p => p.Tasks.Any())
                .Select(p =>
                    new ProjectDto
                    {
                        TasksCount = p.Tasks.Count,
                        ProjectName = p.Name,
                        HasEndDate = p.DueDate == null ? "No" : "Yes",
                        Tasks = p.Tasks.Select(t =>
                                new TaskDto
                                {
                                    Name = t.Name,
                                    Label = t.LabelType.ToString()
                                })
                            .OrderBy(t => t.Name)
                            .ToArray()
                    })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ProjectDto[]), 
                new XmlRootAttribute("Projects"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });
	
            serializer.Serialize(new StringWriter(sb), projects, namespaces);
	
            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate.Date >= date))
                .OrderByDescending(e => e.EmployeesTasks.Sum(et => et.Task.Id))
                .ThenBy(e => e.Username)
                .Select(e =>
                    new EmployeeDto
                    {
                        Username = e.Username,
                        Tasks = e.EmployeesTasks
                            //.OrderByDescending(et => et.Task.DueDate)
                            //.ThenBy(et => et.Task.Name)
                            .Select(et =>
                            new TaskFullDto
                            {
                                TaskName = et.Task.Name,
                                OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                                DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                                LabelType = et.Task.LabelType.ToString(),
                                ExecutionType = et.Task.ExecutionType.ToString()
                            })
                            .OrderByDescending(t => DateTime.ParseExact(t.DueDate, "d", CultureInfo.InvariantCulture))
                            .ThenBy(t => t.TaskName)
                            .ToArray()
                    })
                //.OrderByDescending(e => e.Tasks.Length)
                //.ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            var json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            return json;
        }
    }
}