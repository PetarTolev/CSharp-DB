namespace Cinema.DataProcessor
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using ImportDto;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var movies = JsonConvert.DeserializeObject<Movie[]>(jsonString)
                .ToArray();

            var validMovies = new List<Movie>();
            var sb = new StringBuilder();

            foreach (var movie in movies)
            {
                if (!IsValid(movie))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validMovies.Add(movie);
                sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre, $"{movie.Rating:F2}"));
            }

            context.Movies.AddRange(validMovies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsDto = JsonConvert.DeserializeObject<HallImportDto[]>(jsonString)
                .ToArray();

            var validHalls = new List<Hall>();
            var sb = new StringBuilder();

            foreach (var hallDto in hallsDto)
            {
                var hall = Mapper.Map<Hall>(hallDto);

                if (!IsValid(hall) || hallDto.Seats < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var seats = Enumerable
                    .Range(0, hallDto.Seats)
                    .Select(x => new Seat())
                    .ToList();
                hall.Seats.AddRange(seats);

                validHalls.Add(hall);

                var type = GetHallType(hall.Is3D, hall.Is4Dx);
                sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, type, hall.Seats.Count));
            }

            context.Halls.AddRange(validHalls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static string GetHallType(bool is3D, bool is4Dx)
        {
            if (is3D && is4Dx)
            {
                return "4Dx/3D";
            }

            if (is3D)
            {
                return "3D";
            }

            if (is4Dx)
            {
                return "4Dx";
            }

            return "Normal";
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProjectionImportDto[]),
                new XmlRootAttribute("Projections"));

            var projectionsDto = (ProjectionImportDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validProjections = new List<Projection>();

            var sb = new StringBuilder();

            foreach (var projectionDto in projectionsDto)
            {
                var projection = Mapper.Map<Projection>(projectionDto);
                var movies = context.Movies
                    .ToList();
                var halls = context.Halls
                    .ToList();

                if (!IsValid(projection) ||
                    movies.All(m => m.Id != projectionDto.MovieId) || 
                    halls.All(h => h.Id != projectionDto.HallId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                projection.Movie = movies.Find(m => m.Id == projectionDto.MovieId);
                projection.Hall = halls.Find(h => h.Id == projectionDto.HallId);

                validProjections.Add(projection);

                sb.AppendLine(string.Format(SuccessfulImportProjection, projection.Movie.Title,
                    projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            }
	
            context.Projections.AddRange(validProjections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(CustomerImportDto[]), 
                new XmlRootAttribute("Customers"));

            var customersDto = (CustomerImportDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validCustomers = new List<Customer>();

            var sb = new StringBuilder();

            foreach (var customerDto in customersDto)
            {
                var customer = Mapper.Map<Customer>(customerDto);

                if (!IsValid(customer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName, customer.Tickets.Count));
                validCustomers.Add(customer);
            }

            context.Customers.AddRange(validCustomers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}