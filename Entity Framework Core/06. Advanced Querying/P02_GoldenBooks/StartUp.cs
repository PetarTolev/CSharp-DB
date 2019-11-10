namespace P02_GoldenBooks
{
    using BookShop.Data;
    using BookShop.Models.Enums;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new BookShopContext())
            {
                Console.WriteLine(GetGoldenBooks(db));
            }
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var titles = context.Books
                .Where(b => b.EditionType == EditionType.Gold)
                .Where(b => b.Copies < 5000)
                .Select(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
