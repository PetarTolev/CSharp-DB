namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> player)
        {
            player
                .HasKey(p => p.PlayerId);

            player
                .HasMany(p => p.PlayerStatistics)
                .WithOne(ps => ps.Player);

            player
                .Property(p => p.Name)
                .IsRequired();
        }
    }
}
