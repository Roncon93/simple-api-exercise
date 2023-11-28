using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Contexts.Configurations
{
    public class CompanyEnityConfiguration : IEntityTypeConfiguration<Company>
    {
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
