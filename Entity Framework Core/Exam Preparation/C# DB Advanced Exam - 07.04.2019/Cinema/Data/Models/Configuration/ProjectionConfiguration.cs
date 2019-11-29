namespace Cinema.Data.Models.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProjectionConfiguration : IEntityTypeConfiguration<Projection>
    {
        public void Configure(EntityTypeBuilder<Projection> projection)
        {
            projection
                .HasOne(p => p.Hall)
                .WithMany(h => h.Projections)
                .HasForeignKey(p => p.HallId);

            projection
                .HasOne(p => p.Movie)
                .WithMany(m => m.Projections)
                .HasForeignKey(p => p.MovieId);
        }
    }
}