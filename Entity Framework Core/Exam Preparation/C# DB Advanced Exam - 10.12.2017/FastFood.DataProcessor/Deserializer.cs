namespace FastFood.DataProcessor
{
    using Data;
    using Dto.Import;
    using Models;
    using Models.Enums;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";
        private const string SuccessOrderMessage = "Order for {0} on {1} added";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
            var employeesDto = JsonConvert.DeserializeObject<EmployeeImportDto[]>(jsonString)
                .ToArray();

            var employees = new List<Employee>();
            var positions = new List<Position>();

            var sb = new StringBuilder();

            foreach (var employeeDto in employeesDto)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var position = positions.FirstOrDefault(p => p.Name == employeeDto.PositionName);

                if (position == null)
                {
                    position = new Position
                    {
                        Name = employeeDto.PositionName
                    };

                    positions.Add(position);
                }

                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = position,
                    PositionId = position.Id
                };

                employees.Add(employee);
                sb.AppendLine(string.Format(SuccessMessage, employee.Name));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var itemsDto = JsonConvert.DeserializeObject<ItemImportDto[]>(jsonString)
                .ToArray();

            var items = new List<Item>();
            var categories = new List<Category>();

            var sb = new StringBuilder();

            foreach (var itemDto in itemsDto)
            {
                if (!IsValid(itemDto) || items.Any(i => i.Name == itemDto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var category = categories.FirstOrDefault(c => c.Name == itemDto.CategoryName);

                if (category == null)
                {
                    category = new Category
                    {
                        Name = itemDto.CategoryName
                    };

                    categories.Add(category);
                }

                var item = new Item
                {
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Category = category,
                    CategoryId = category.Id
                };

                items.Add(item);
                sb.AppendLine(string.Format(SuccessMessage, item.Name));
            }

            context.Items.AddRange(items);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

		public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
            var serializer = new XmlSerializer(typeof(OrderImportDto[]),
                new XmlRootAttribute("Orders"));

            var ordersDto = (OrderImportDto[]) serializer.Deserialize(new StringReader(xmlString));

            var orders = new List<Order>();

            var sb = new StringBuilder();

            foreach (var orderDto in ordersDto)
            {
                var isValidDto = IsValid(orderDto);
                var employee = context.Employees.FirstOrDefault(e => e.Name == orderDto.EmployeeName);
                var itemNames = context.Items.Select(i => i.Name).ToList();
                var isValidItems = orderDto.Items.Select(i => i.Name).All(x => itemNames.Contains(x));
                var isValidOrderType = Enum.IsDefined(typeof(OrderType), orderDto.Type);

                if (!isValidDto ||
                    employee == null ||
                    !isValidOrderType || 
                    !isValidItems)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var order = new Order
                {
                    Customer = orderDto.CustomerName,
                    DateTime = DateTime.ParseExact(orderDto.DateTime, @"dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Employee = employee,
                    EmployeeId = employee.Id,
                    Type = (OrderType)Enum.Parse(typeof(OrderType), orderDto.Type),
                    OrderItems = orderDto.Items.Select(i => new OrderItem
                    {
                        Item = context.Items
                            .FirstOrDefault(t => t.Name == i.Name),
                        Quantity = i.Quantity
                    })
                        .ToList()

                };

                sb.AppendLine(string.Format(SuccessOrderMessage, order.Customer, order.DateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)));
                orders.Add(order);
            }
	
            context.Orders.AddRange(orders);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
	}
}