namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> position)
        {
            position
                .HasKey(p => p.PositionId);

            position
                .HasMany(po => po.Players)
                .WithOne(pi => pi.Position);

            position
                .Property(p => p.Name)
                .IsRequired();
        }
    }
}
