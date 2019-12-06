using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using Cinema.Data.Models;
using Cinema.DataProcessor.ImportDto;
using Newtonsoft.Json;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Cinema.DataProcessor
{
    using System;

    using Data;

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
            var moviesDto = JsonConvert.DeserializeObject<MovieDto[]>(jsonString).ToArray();

            var validMovies = new List<Movie>();
            var sb = new StringBuilder();

            foreach (var movieDto in moviesDto)
            {
                var movie = Mapper.Map<Movie>(movieDto);

                if (!IsValid(movie))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validMovies.Add(movie);
                sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre, movie.Rating.ToString("F2")));
            }

            context.Movies.AddRange(validMovies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsDto = JsonConvert.DeserializeObject<HallDto[]>(jsonString).ToArray();

            var validHalls = new List<Hall>();
            var sb = new StringBuilder();

            foreach (var hallDto in hallsDto)
            {
                if (hallDto.Seats < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = Mapper.Map<Hall>(hallDto);

                if (!IsValid(hall))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projectionType = GetProjectionTime(hall.Is4Dx, hall.Is3D);

                sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats.Count));
                validHalls.Add(hall);
            }

            context.Halls.AddRange(validHalls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static string GetProjectionTime(bool is4Dx, bool is3D)
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
            var serializer = new XmlSerializer(typeof(ProjectionDto[]), 
                new XmlRootAttribute("Projections"));

            var projectionsDto = (ProjectionDto[])serializer.Deserialize(new StringReader(xmlString));
            
            var validProjections = new List<Projection>();
            var sb = new StringBuilder();

            foreach (var projectionDto in projectionsDto)
            {
                var projection = Mapper.Map<Projection>(projectionDto);

                var movieIds = context.Movies.Select(m => m.Id).ToArray();
                var hallIds = context.Halls.Select(h => h.Id).ToArray();

                if (!IsValid(projection) || 
                    !movieIds.Contains(projection.MovieId) ||
                    !hallIds.Contains(projection.HallId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validProjections.Add(projection);

                var movieTitle = context.Movies.First(m => m.Id == projection.MovieId).Title;

                sb.AppendLine(string.Format(SuccessfulImportProjection, movieTitle,
                    projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            }

            context.Projections.AddRange(validProjections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(CustomerDto[]), 
                new XmlRootAttribute("Customers"));

            var customersDto = (CustomerDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validCustomers = new List<Customer>();
            var sb = new  StringBuilder();

            foreach (var customerDto in customersDto)
            {
                var customer = Mapper.Map<Customer>(customerDto);

                if (!IsValid(customer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validCustomers.Add(customer);

                sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, 
                    customer.FirstName, customer.LastName, customer.Tickets.Count));
            }

            context.Customers.AddRange(validCustomers);
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