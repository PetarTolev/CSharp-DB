namespace SoftUni
{
    using Data;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            using (context)
            {
                var result = DeleteProjectById(context);

                Console.WriteLine(result);
            }
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectForDelete = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            var employeeProjectForDelete = context.EmployeesProjects
                .Where(p => p.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeeProjectForDelete);
            context.Projects.Remove(projectForDelete);

            context.SaveChanges();

            var projectNames = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var projectName in projectNames)
            {
                sb.AppendLine(projectName);
            }

            return sb.ToString().TrimEnd();
        }
    }
}