namespace P03_BooksByPrice
{
    using System;
    using System.Linq;
    using System.Text;
    using BookShop.Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new BookShopContext())
            {
                Console.WriteLine(GetBooksByPrice(db));
            }
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
