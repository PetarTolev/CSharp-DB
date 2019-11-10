namespace P01_AgeRestriction
{
    using BookShop.Data;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            string input = Console.ReadLine();

            using (var db = new BookShopContext())
            {
                var result = GetBooksByAgeRestriction(db, input);

                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var bookTitles = context    
                .Books
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => new
                {
                    BookTitle = b.Title
                })
                .OrderBy(b => b.BookTitle)
                .ToList();

            var sb = new StringBuilder();

            foreach (var bookTitle in bookTitles)
            {
                sb.AppendLine(bookTitle.BookTitle);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
