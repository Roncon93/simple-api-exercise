using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts.Configurations;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Contexts
{
    /// <summary>
    /// A Sqlite implementation of the <see cref="DbContext"/> for the <see cref="Employee"/> entity.
    /// </summary>
    public class EmployeeSqliteDbContext : DbContext
    {
        public EmployeeSqliteDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyEnityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEnityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeDepartmentEnityConfiguration());
        }
    }
}
