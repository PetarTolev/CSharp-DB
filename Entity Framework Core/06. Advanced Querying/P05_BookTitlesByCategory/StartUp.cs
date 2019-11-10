namespace P05_BookTitlesByCategory
{
    using System;
    using System.Linq;
    using BookShop.Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            string input = Console.ReadLine();

            using (var db = new BookShopContext())
            {
                Console.WriteLine(GetBooksByCategory(db, input));
            }
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var inputCategories = input.ToLower().Split(' ').ToList();

            var books = context.Books
                .Where(b => b.BookCategories
                    .Any(c => inputCategories
                        .Contains(c.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            return string.Join(Environment.NewLine, books);
        }
    }
}
