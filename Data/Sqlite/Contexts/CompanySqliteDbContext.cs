using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Contexts
{
    public class CompanySqliteDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Company>();
            
            entity.ToTable("Companies");

            entity.HasKey(c => c.Id);

            entity.HasMany(c => c.Employees).WithOne(e => e.Company);
        }
    }
}
