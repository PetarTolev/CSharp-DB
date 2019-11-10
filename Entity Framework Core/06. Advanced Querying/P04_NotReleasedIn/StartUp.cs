namespace P04_NotReleasedIn
{
    using BookShop.Data;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            int date = int.Parse(Console.ReadLine());

            using (var db = new BookShopContext())
            {
                Console.WriteLine(GetBooksNotReleasedIn(db, date));
            }
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    Title = b.Title,
                    Id = b.BookId
                })
                .OrderBy(b => b.Id)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
