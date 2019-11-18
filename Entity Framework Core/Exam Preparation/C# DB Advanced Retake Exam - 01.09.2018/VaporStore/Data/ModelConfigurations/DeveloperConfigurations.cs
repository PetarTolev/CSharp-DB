namespace VaporStore.Data.ModelConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class DeveloperConfigurations : IEntityTypeConfiguration<Developer>
    {
        public void Configure(EntityTypeBuilder<Developer> developer)
        {
            developer.HasKey(d => d.Id);
        }
    }
}
