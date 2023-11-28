using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Contexts.Configurations
{
    /// <summary>
    /// Configures the <see cref="Company"/> database entity.
    /// </summary>
    public class CompanyEnityConfiguration : IEntityTypeConfiguration<Company>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Employees)
                .WithOne(c => c.Company)
                .HasForeignKey(e => e.CompanyId)
                .HasPrincipalKey(c => c.Id);
        }
    }
}
