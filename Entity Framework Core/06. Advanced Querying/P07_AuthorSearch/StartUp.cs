namespace P07_AuthorSearch
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
                Console.WriteLine(GetAuthorNamesEndingIn(db, input));
            }
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var names = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}"
                })
                .OrderBy(a => a.FullName);

            return string.Join(Environment.NewLine, names.Select(n => n.FullName));
        }
    }
}
