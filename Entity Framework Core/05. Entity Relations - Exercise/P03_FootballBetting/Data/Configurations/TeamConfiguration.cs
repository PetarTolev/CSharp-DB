namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> team)
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
        }
    }
}
