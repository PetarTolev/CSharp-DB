namespace VaporStore.Data
{
    using Microsoft.EntityFrameworkCore;
    using ModelConfigurations;
    using Models;

    public class VaporStoreDbContext : DbContext
	{
        public DbSet<Card> Cards { get; set; }

        public DbSet<Developer> Developers { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameTag> GameTags { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Tag> Tags { get; set; }
        
        public DbSet<User> Users { get; set; }

		public VaporStoreDbContext()
		{
		}

		public VaporStoreDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if (!options.IsConfigured)
			{
				options
					.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder model)
        {
            model.ApplyConfiguration(new CardConfigurations());
            model.ApplyConfiguration(new DeveloperConfigurations());
            model.ApplyConfiguration(new GameConfigurations());
            model.ApplyConfiguration(new GameTagConfigurations());
            model.ApplyConfiguration(new GenresConfigurations());
            model.ApplyConfiguration(new PurchaseConfigurations());
            model.ApplyConfiguration(new TagConfigurations());
            model.ApplyConfiguration(new UserConfigurations());
        }
	}
}