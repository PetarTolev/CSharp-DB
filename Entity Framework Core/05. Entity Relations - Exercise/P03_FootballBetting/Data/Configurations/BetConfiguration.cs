namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> bet)
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
        }
    }
}
