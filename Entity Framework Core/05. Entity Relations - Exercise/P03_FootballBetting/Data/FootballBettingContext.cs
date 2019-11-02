namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FootballBettingContext : DbContext
    {
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
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-IN4GT0T\SQLEXPRESS;Database=FootballBetting;Integrated Security=true;");
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
            modelBuilder.Entity<User>(user => { user.HasKey(u => u.UserId); });
        }

        private void ConfigureTownEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Town>(town => { town.HasKey(t => t.TownId); });
        }

        private void ConfigureTeamEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(team => { team.HasKey(t => t.TeamId); });
        }

        private void ConfigurePositionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>(position => { position.HasKey(p => p.PositionId); });
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
                player.HasKey(p => p.PlayerId);

                player.HasMany(p => p.PlayerStatistics)
                    .WithOne(ps => ps.Player);
            });
        }

        private void ConfigureGameEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(game =>
            {
                game.HasKey(g => g.GameId);

                game.HasMany(g => g.PlayerStatistics)
                    .WithOne(ps => ps.Game);

                game.HasOne(g => g.HomeTeam)
                    .WithMany(ht => ht.HomeGames);
            });
        }

        private void ConfigureCountryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(country => { country.HasKey(c => c.CountryId); });
        }

        private void ConfigureColorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>(color =>
            {
                color.HasKey(c => c.ColorId);

                color.HasMany(c => c.PrimaryKitTeams)
                    .WithOne(pkt => pkt.PrimaryKitColor);

                color.HasMany(c => c.SecondaryKitTeams)
                    .WithOne(skt => skt.SecondaryKitColor);
            });
        }

        private void ConfigureBetEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bet>(bet => { bet.HasKey(b => b.BetId); });
        }
    }
}
