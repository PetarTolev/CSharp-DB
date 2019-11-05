namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> country)
        {
            country
                .HasKey(c => c.CountryId);

            country
                .HasMany(c => c.Towns)
                .WithOne(t => t.Country);
        }
    }
}
