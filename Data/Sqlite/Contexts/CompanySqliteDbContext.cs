using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts.Configurations;

namespace SimpleApiProject.Data.Sqlite.Contexts
{
    public class CompanySqliteDbContext : DbContext
    {
        public CompanySqliteDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyEnityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEnityConfiguration());
        }
    }
}
