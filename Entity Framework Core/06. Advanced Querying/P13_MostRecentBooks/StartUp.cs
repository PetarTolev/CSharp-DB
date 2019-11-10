namespace P13_MostRecentBooks
{
    using BookShop.Data;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new BookShopContext())
            {
                Console.WriteLine(GetMostRecentBooks(db));
            }
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                        .Select(cb => new
                        {
                            Title = cb.Book.Title,
                            ReleaseDate = cb.Book.ReleaseDate
                        })
                        .OrderByDescending(cb => cb.ReleaseDate)
                        .Take(3)
                })
                .OrderBy(c => c.CategoryName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
