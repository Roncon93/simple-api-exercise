using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Contexts.Configurations
{
    public class EmployeeEnityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => new { e.CompanyId, e.EmployeeNumber });

            builder.Ignore(e => e.Managers)
                .Ignore(e => e.EmployeeFullName);
        }
    }
}
