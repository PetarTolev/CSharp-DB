namespace P10_CountBooks
{
    using System;
    using System.Linq;
    using BookShop.Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var num = int.Parse(Console.ReadLine());

            using (var db = new BookShopContext())
            {
                Console.WriteLine(CountBooks(db, num));
            }
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToList();

            return books.Count;
        }
    }
}
