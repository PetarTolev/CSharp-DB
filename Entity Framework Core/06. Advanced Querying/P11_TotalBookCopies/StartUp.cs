namespace P11_TotalBookCopies
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
                Console.WriteLine(CountCopiesByAuthor(db));
            }
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    Copies = a.Books.Sum(b => b.Copies),
                    Name = $"{a.FirstName} {a.LastName}"
                })
                .OrderByDescending(a => a.Copies)
                .ToList();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.Name} - {author.Copies}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
