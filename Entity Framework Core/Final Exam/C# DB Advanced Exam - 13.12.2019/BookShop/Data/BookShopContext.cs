namespace BookShop.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class BookShopContext : DbContext
    {
        public BookShopContext() { }

        public BookShopContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }

        public DbSet<AuthorBook> AuthorsBooks { get; set; }

        public DbSet<Author> Authors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorBook>(ab => ab.HasKey(x => new {x.AuthorId, x.BookId}));

            modelBuilder.Entity<AuthorBook>(ab => ab
                .HasOne(a => a.Author)
                .WithMany(b => b.AuthorsBooks)
                .HasForeignKey(a => a.AuthorId));

            modelBuilder.Entity<AuthorBook>(ab => ab
                .HasOne(a => a.Book)
                .WithMany(b => b.AuthorsBooks)
                .HasForeignKey(a => a.BookId));
        }
    }
}