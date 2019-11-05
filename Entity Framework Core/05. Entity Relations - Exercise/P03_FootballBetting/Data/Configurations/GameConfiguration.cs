namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> game)
        {
            game
                .HasKey(g => g.GameId);

            game
                .HasMany(g => g.PlayerStatistics)
                .WithOne(ps => ps.Game);

            game
                .HasOne(g => g.HomeTeam)
                .WithMany(ht => ht.HomeGames);
        }
    }
}
