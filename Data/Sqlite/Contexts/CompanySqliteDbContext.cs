using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts.Configurations;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Contexts
{
    /// <summary>
    /// A Sqlite implementation of the <see cref="DbContext"/> for the <see cref="Company"/> entity.
    /// </summary>
    public class CompanySqliteDbContext : DbContext
    {
        public CompanySqliteDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyEnityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEnityConfiguration());
        }
    }
}
