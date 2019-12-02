namespace FastFood.DataProcessor
{
    using Data;
    using Dto.Export;
    using Models.Enums;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
	{
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var type = Enum.Parse<OrderType>(orderType);

            var employee = context.Employees
                .Where(e => e.Name == employeeName)
                .Select(e =>
                    new EmployeeExportDto
                    {
                        Name = e.Name,
                        Orders = e.Orders
                            .Where(o => type == o.Type)
                            .Select(o =>
                                new OrderExportDto
                                {
                                    Customer = o.Customer,
                                    Items = o.OrderItems.Select(oi =>
                                            new ItemExportDto
                                            {
                                                Name = oi.Item.Name,
                                                Price = oi.Item.Price,
                                                Quantity = oi.Quantity

                                            })
                                        .ToArray(),
                                    TotalPrice = o.OrderItems.Sum(oi => oi.Quantity * oi.Item.Price)
                                })
                            .OrderByDescending(o => o.TotalPrice)
                            .ThenByDescending(o => o.Items.Length)
                            .ToArray(),
                        TotalMade = e.Orders
                            .Where(o => type == o.Type)
                            .Sum(o => o.OrderItems.Sum(oi => oi.Item.Price * oi.Quantity))
                    }
                )
                .SingleOrDefault();

            var json = JsonConvert.SerializeObject(employee, Formatting.Indented);
            return json;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            var inputCategories = categoriesString.Split(',').ToArray();

            var categories = context.Items
                .Where(i => inputCategories.Any(c => c == i.Category.Name))
                .GroupBy(i => i.Category.Name)
                .Select(g =>
                    new CategoryExportDto
                    {
                        Name = g.Key,
                        MostPopularItem = g.Select(i =>
                                new MostPopularItemDto
                                {
                                    Name = i.Name,
                                    TotalMade = i.OrderItems.Sum(oi => oi.Quantity * oi.Item.Price),
                                    TimesSold = i.OrderItems.Sum(oi => oi.Quantity)
                                })
                            .OrderByDescending(i => i.TotalMade)
                            .ThenByDescending(i => i.TimesSold)
                            .First()
                    })
                .OrderByDescending(dto => dto.MostPopularItem.TotalMade)
                .ThenByDescending(dto => dto.MostPopularItem.TimesSold)
                .ToArray();

             var serializer = new XmlSerializer(typeof(CategoryExportDto[]),
                new XmlRootAttribute("Categories"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new [] {new XmlQualifiedName("", "") });
            serializer.Serialize(new StringWriter(sb), categories, namespaces);

            var result = sb.ToString();

            return result;
        }
    }
}