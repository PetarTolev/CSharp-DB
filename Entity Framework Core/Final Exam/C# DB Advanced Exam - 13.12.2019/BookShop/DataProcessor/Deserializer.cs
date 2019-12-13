namespace BookShop.DataProcessor
{
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;
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

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(BookDto[]),
                new XmlRootAttribute("Books"));

            var booksDto = (BookDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validBooks = new List<Book>();

            var sb = new StringBuilder();

            foreach (var bookDto in booksDto)
            {
                var book = new Book
                {
                    Name = bookDto.Name,
                    Genre = Enum.Parse<Genre>(bookDto.Genre),
                    Price = decimal.Parse(bookDto.Price),
                    Pages = int.Parse(bookDto.Pages),
                    PublishedOn = DateTime.ParseExact(bookDto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                };

                var validGenre = Enum.IsDefined(typeof(Genre), book.Genre);

                if (!IsValid(book) || !validGenre)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validBooks.Add(book);
                sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }

            context.Books.AddRange(validBooks);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsDto = JsonConvert.DeserializeObject<AuthorDto[]>(jsonString).ToArray();
            
            var validAuthors = new List<Author>();
            var sb = new StringBuilder();
            
            var books = context.Books.Select(b => b.Id).ToList();

            foreach (var authorDto in authorsDto)
            {
                var emails = validAuthors.Select(a => a.Email).ToList();

                var author = new Author
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Phone = authorDto.Phone,
                    Email = authorDto.Email
                };

                if (!IsValid(author) || emails.Contains(author.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var bookId in authorDto.BookIds)
                {
                    if (bookId.Id != null && books.Contains((int)bookId.Id))
                    {
                        var authorBook = new AuthorBook
                        {
                            BookId = (int)bookId.Id
                        };

                        author.AuthorsBooks.Add(authorBook);
                    }
                }

                if (author.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var fullName = $"{author.FirstName} {author.LastName}";

                validAuthors.Add(author);
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, fullName, author.AuthorsBooks.Count));
            }
            
            context.Authors.AddRange(validAuthors);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}