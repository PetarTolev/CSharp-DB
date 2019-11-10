namespace P09_BookSearchByAuthor
{
    using System;
    using System.Linq;
    using System.Text;
    using BookShop.Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var input = Console.ReadLine().ToLower();

            using (var db = new BookShopContext())
            {
                Console.WriteLine(GetBooksByAuthor(db, input));
            }
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    AuthorName = $"{b.Author.FirstName} {b.Author.LastName}"
                });

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.BookTitle} ({book.AuthorName})");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
