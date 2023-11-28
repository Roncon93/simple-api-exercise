using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Repositories
{
    /// <summary>
    /// A Sqlite implementation of a data store repository for the <see cref="EmployeeDepartment"/> entity.
    /// </summary>
    public class EmployeeDepartmentRepository : BaseRepository<EmployeeDepartment, EmployeeSqliteDbContext>, IRepository<EmployeeDepartment>
    {
        public EmployeeDepartmentRepository(ILogger<BaseRepository<EmployeeDepartment, EmployeeSqliteDbContext>> logger, IDbContextFactory<EmployeeSqliteDbContext> contextFactory)
            : base(logger, contextFactory)
        {
        }

        /// <inheritdoc/>
        public override IQueryable<EmployeeDepartment> GetSet(DbContext context) =>
            context.Set<EmployeeDepartment>().Include(e => e.Employees);
    }
}
