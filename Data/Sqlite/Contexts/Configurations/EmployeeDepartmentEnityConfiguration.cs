using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Contexts.Configurations
{
    /// <summary>
    /// Configures the <see cref="EmployeeDepartment"/> database entity.
    /// </summary>
    public class EmployeeDepartmentEnityConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
        {
            builder.ToTable("EmployeeDepartments");

            builder.HasKey(e => new { e.Id });

            builder.HasMany(c => c.Employees)
                .WithOne(c => c.Department)
                .HasForeignKey(e => e.DepartmentId)
                .HasPrincipalKey(c => c.Id);
        }
    }
}
