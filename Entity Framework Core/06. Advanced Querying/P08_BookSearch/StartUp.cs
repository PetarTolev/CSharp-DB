namespace P08_BookSearch
{
    using System;
    using System.Linq;
    using BookShop.Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var input = Console.ReadLine().ToLower();

            using (var db = new BookShopContext())
            {
                Console.WriteLine(GetBookTitlesContaining(db, input));
            }
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }
    }
}
