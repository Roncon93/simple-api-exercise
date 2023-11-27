using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Contexts
{
    public class EmployeeSqliteDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Employee>();
            
            entity.ToTable("Employees");

            entity.HasKey(e => new { e.Company!.Id, e.EmployeeNumber });

            entity.Property(e => e.Company).IsRequired();

            entity.HasOne(e => e.Company).WithMany(c => c.Employees);
        }
    }
}
