namespace P14_IncreasePrices
{
    using System.Linq;
    using BookShop.Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new BookShopContext())
            {
                IncreasePrices(db);
            }
        }

        public static void IncreasePrices(BookShopContext context)
        {
            context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList()
                .ForEach(b => b.Price += 5);
        }
    }
}
