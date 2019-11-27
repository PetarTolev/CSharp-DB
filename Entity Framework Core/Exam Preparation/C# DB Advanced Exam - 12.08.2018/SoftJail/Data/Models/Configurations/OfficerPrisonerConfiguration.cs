namespace SoftJail.Data.Models.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OfficerPrisonerConfiguration : IEntityTypeConfiguration<OfficerPrisoner>
    {
        public void Configure(EntityTypeBuilder<OfficerPrisoner> officerPrisoner)
        {
            officerPrisoner.HasKey(op => new {op.OfficerId, op.PrisonerId});

            officerPrisoner.HasOne(op => op.Prisoner)
                .WithMany(p => p.PrisonerOfficers)
                .HasForeignKey(op => op.PrisonerId)
                .OnDelete(DeleteBehavior.Restrict);

            officerPrisoner.HasOne(op => op.Officer)
                .WithMany(p => p.OfficerPrisoners)
                .HasForeignKey(op => op.OfficerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
