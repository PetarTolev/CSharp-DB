using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BookShop.Data.Models.Enums;

namespace BookShop.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using Formatting = Newtonsoft.Json.Formatting;
    using ExportDto;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new MostCraziestAuthorDto
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    Books = a.AuthorsBooks
                        .Select(ab =>
                            new BookDto
                            {
                                BookName = ab.Book.Name,
                                BookPrice = ab.Book.Price.ToString("F2")
                            })
                        .OrderByDescending(b => decimal.Parse(b.BookPrice)) //todo: decimal
                        .ToArray()
                })
                .ToArray()
                .OrderByDescending(a => a.Books.Length)
                .ThenBy(a => a.AuthorName)
                .ToArray();

            var json = JsonConvert.SerializeObject(authors, Formatting.Indented);
            return json;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var books = context.Books
                .Where(b => b.PublishedOn < date && b.Genre == Genre.Science)
                .OrderByDescending(b => b.Pages)
                .ThenByDescending(b => b.PublishedOn)
                .Select(b => new OldesBookDto
                {
                    Pages = b.Pages.ToString(),
                    Name = b.Name,
                    Date = b.PublishedOn.ToString("d", CultureInfo.InvariantCulture)
                })
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(OldesBookDto[]), 
                new XmlRootAttribute("Books"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });
	
            serializer.Serialize(new StringWriter(sb), books, namespaces);
	
            return sb.ToString().TrimEnd();
        }
    }
}