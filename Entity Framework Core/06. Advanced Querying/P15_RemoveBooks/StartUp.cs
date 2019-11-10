namespace P15_RemoveBooks
{
    using System;
    using System.Linq;
    using BookShop.Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new BookShopContext())
            {
                Console.WriteLine(RemoveBooks(db));
            }
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.RemoveRange(books);
            context.SaveChanges();

            return books.Count;
        }
    }
}
