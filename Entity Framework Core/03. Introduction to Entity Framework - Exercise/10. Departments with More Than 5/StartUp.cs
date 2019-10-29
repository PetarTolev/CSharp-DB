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
                var result = GetDepartmentsWithMoreThan5Employees(context);

                Console.WriteLine(result);
            }
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    d.Employees,
                    d.Name,
                    ManagerName = d.Manager.FirstName + ' ' + d.Manager.LastName,
                    EmployeeInfo = d.Employees.Select(e => e.FirstName + ' ' + e.LastName + " - " + e.JobTitle)
                })
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .ToList();

            var sb = new StringBuilder();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.Name} - {department.ManagerName}");

                foreach (var employee in department.EmployeeInfo)
                {
                    sb.AppendLine(employee);
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}