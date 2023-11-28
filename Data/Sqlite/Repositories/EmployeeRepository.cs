using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Repositories
{
    /// <summary>
    /// A Sqlite implementation of a data store repository for the <see cref="Employee"/> entity.
    /// </summary>
    public class EmployeeRepository : BaseRepository<Employee, EmployeeSqliteDbContext>, IRepository<Employee>
    {
        public EmployeeRepository(ILogger<BaseRepository<Employee, EmployeeSqliteDbContext>> logger, IDbContextFactory<EmployeeSqliteDbContext> contextFactory)
            : base(logger, contextFactory)
        {
        }

        /// <inheritdoc/>
        public override IQueryable<Employee> GetSet(DbContext context) =>
            context.Set<Employee>().Include(e => e.Company).Include(e => e.Department);
    }
}
