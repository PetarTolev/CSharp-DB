namespace Cinema.DataProcessor
{
    using Data;
    using ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                .Where(r => r.Rating >= rating && r.Projections.Any(t => t.Tickets.Count >= 1))
                .OrderByDescending(r => r.Rating)
                .ThenByDescending(p => p.Projections.Sum(t => t.Tickets.Sum(pc => pc.Price)))
                .Select(m => new TopMovieExportDto
                {
                    MovieName = m.Title,
                    Rating = m.Rating.ToString("F2"),
                    TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("F2"),
                    Customers = m.Projections.SelectMany(t => t.Tickets).Select(c => new CustomerExportDto
                    {
                        FirstName = c.Customer.FirstName,
                        LastName = c.Customer.LastName,
                        Balance = c.Customer.Balance.ToString("F2")
                    })
                        .OrderByDescending(b => b.Balance)
                        .ThenBy(f => f.FirstName)
                        .ThenBy(l => l.LastName)
                        .ToArray()
                })
                .Take(10)
                .ToList();

            var jsonString = JsonConvert.SerializeObject(movies, Newtonsoft.Json.Formatting.Indented);

            return jsonString;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context
                .Customers
                .Where(a => a.Age >= age)
                .OrderByDescending(x => x.Tickets.Sum(p => p.Price))
                .Take(10)
                .Select(x => new TopCustomerExportDto()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SpentMoney = x.Tickets.Sum(p => p.Price).ToString("F2"),
                    SpentTime = TimeSpan.FromSeconds(
                            x.Tickets.Sum(s => s.Projection.Movie.Duration.TotalSeconds))
                        .ToString(@"hh\:mm\:ss")
                })
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(TopCustomerExportDto[]), new XmlRootAttribute("Customers"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new [] {XmlQualifiedName.Empty});
            xmlSerializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}