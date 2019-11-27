namespace SoftJail.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Configurations;

    public class SoftJailDbContext : DbContext
	{
        public DbSet<Cell> Cells { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Mail> Mails { get; set; }

        public DbSet<Officer> Officers { get; set; }

        public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }

        public DbSet<Prisoner> Prisoners { get; set; }

        public SoftJailDbContext()
		{
		}

		public SoftJailDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MailConfiguration());
            builder.ApplyConfiguration(new OfficerPrisonerConfiguration());
            builder.ApplyConfiguration(new PrisonerConfiguration());
            builder.ApplyConfiguration(new CellConfiguration());
        }
	}
}