namespace P06_ReleasedBeforeDate
{
    using BookShop.Data;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            string input = Console.ReadLine();

            using (var db = new BookShopContext())
            {
                Console.WriteLine(GetBooksReleasedBefore(db, input));
            }
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var format = "dd-MM-yyyy";
            var parsedDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    Title = b.Title,
                    Type = b.EditionType,
                    Price = b.Price
                });

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.Type} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
