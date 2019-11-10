namespace P12_ProfitByCategory
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
                Console.WriteLine(GetTotalProfitByCategory(db));
            }
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    TotalMoney = c.CategoryBooks.Select(cb => cb.Book.Copies * cb.Book.Price).Sum(),
                    Name = c.Name
                })
                .OrderByDescending(c => c.TotalMoney)
                .ToList();

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.Name} ${category.TotalMoney:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
