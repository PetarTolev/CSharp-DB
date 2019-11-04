namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBetEntity(modelBuilder);

            ConfigureColorEntity(modelBuilder);

            ConfigureCountryEntity(modelBuilder);

            ConfigureGameEntity(modelBuilder);

            ConfigurePlayerEntity(modelBuilder);

            ConfigurePlayerStatisticEntity(modelBuilder);

            ConfigurePositionEntity(modelBuilder);

            ConfigureTeamEntity(modelBuilder);

            ConfigureTownEntity(modelBuilder);

            ConfigureUserEntity(modelBuilder);
        }

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.UserId);

                user
                    .HasMany(u => u.Bets)
                    .WithOne(b => b.User);
            });
        }

        private void ConfigureTownEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Town>(town =>
            {
                town.HasKey(t => t.TownId);

                town
                    .HasMany(to => to.Teams)
                    .WithOne(te => te.Town);
            });
        }

        private void ConfigureTeamEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(team =>
            {
                team.HasKey(t => t.TeamId);

                team
                    .HasMany(t => t.Players)
                    .WithOne(p => p.Team);

                team
                    .HasMany(t => t.HomeGames)
                    .WithOne(g => g.HomeTeam)
                    .OnDelete(DeleteBehavior.Restrict);

                team
                    .HasMany(t => t.AwayGames)
                    .WithOne(g => g.AwayTeam)
                    .OnDelete(DeleteBehavior.Restrict);

                team
                    .Property(t => t.Name)
                    .IsRequired();
            });
        }

        private void ConfigurePositionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>(position =>
            {
                position
                    .HasKey(p => p.PositionId);

                position
                    .HasMany(po => po.Players)
                    .WithOne(pi => pi.Position);

                position
                    .Property(p => p.Name)
                    .IsRequired();
            });
        }

        private void ConfigurePlayerStatisticEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>(playerStatistic =>
            {
                playerStatistic.HasKey(ps => new {ps.PlayerId, ps.GameId});
            });
        }

        private void ConfigurePlayerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(player =>
            {
                player
                    .HasKey(p => p.PlayerId);

                player
                    .HasMany(p => p.PlayerStatistics)
                    .WithOne(ps => ps.Player);

                player
                    .Property(p => p.Name)
                    .IsRequired();
            });
        }

        private void ConfigureGameEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(game =>
            {
                game
                    .HasKey(g => g.GameId);

                game
                    .HasMany(g => g.PlayerStatistics)
                    .WithOne(ps => ps.Game);

                game
                    .HasOne(g => g.HomeTeam)
                    .WithMany(ht => ht.HomeGames);
            });
        }

        private void ConfigureCountryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(country =>
            {
                country
                    .HasKey(c => c.CountryId);

                country
                    .HasMany(c => c.Towns)
                    .WithOne(t => t.Country);
            });
        }

        private void ConfigureColorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>(color =>
            {
                color.HasKey(c => c.ColorId);

                color
                    .HasMany(c => c.PrimaryKitTeams)
                    .WithOne(t => t.PrimaryKitColor)
                    .OnDelete(DeleteBehavior.Restrict);

                color
                    .HasMany(c => c.SecondaryKitTeams)
                    .WithOne(t => t.SecondaryKitColor)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureBetEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bet>(bet =>
            {
                bet
                    .HasKey(b => b.BetId);

                bet
                    .Property(b => b.Prediction)
                    .IsRequired();

                bet
                    .Property(b => b.Amount)
                    .HasColumnType("MONEY")
                    .IsRequired();
            });
        }
    }
}
