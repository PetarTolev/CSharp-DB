namespace MiniORM.App
{
    using System.Linq;
    using Data.Entities;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var connectionString = "Server=DESKTOP-IN4GT0T\\SQLEXPRESS;Database=MiniORM;Integrated Security=true;";

            var context = new SoftUniDbContext(connectionString);

            var employee = new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true
            };

            context.Employees.Add(employee);

            var lastEmployee = context.Employees.Last();
            lastEmployee.FirstName = "Modified";

            context.SaveChanges();
        }
    }
}
