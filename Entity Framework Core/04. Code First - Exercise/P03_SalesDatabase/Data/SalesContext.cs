namespace P03_SalesDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class SalesContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCustomerEntity(modelBuilder);

            ConfigureProductEntity(modelBuilder);

            ConfigureSaleEntity(modelBuilder);

            ConfigureStoreEntity(modelBuilder);
        }

        private void ConfigureStoreEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>(store =>
            {
                store.HasKey(s => s.StoreId);

                store.Property(s => s.Name)
                    .HasMaxLength(80)
                    .IsUnicode();
            });
        }

        private void ConfigureSaleEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(sale =>
            {
                sale.HasKey(s => s.SaleId);

                sale.HasOne(s => s.Product)
                    .WithMany(p => p.Sales);

                sale.HasOne(s => s.Customer)
                    .WithMany(c => c.Sales);

                sale.HasOne(s => s.Store)
                    .WithMany(s => s.Sales);

                sale.Property(s => s.Date)
                    .HasDefaultValueSql("GETDATE()");
            });
        }

        private void ConfigureProductEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(product =>
            {
                product.HasKey(p => p.ProductId);

                product.Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                product.Property(p => p.Description)
                    .HasMaxLength(250)
                    .HasDefaultValue("No description");
            });
        }

        private void ConfigureCustomerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(customer =>
                {
                    customer.HasKey(c => c.CustomerId);

                    customer.Property(c => c.Name)
                        .HasMaxLength(100)
                        .IsUnicode();

                    customer.Property(c => c.Email)
                        .HasMaxLength(80);
                });
        }
    }
}
