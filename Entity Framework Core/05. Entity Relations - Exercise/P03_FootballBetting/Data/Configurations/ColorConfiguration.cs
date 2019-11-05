namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> color)
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
        }
    }
}
