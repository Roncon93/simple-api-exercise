using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Repositories
{
    public class EmployeeDepartmentRepository : BaseRepository<EmployeeDepartment, EmployeeSqliteDbContext>, IRepository<EmployeeDepartment>
    {
        public EmployeeDepartmentRepository(ILogger<BaseRepository<EmployeeDepartment, EmployeeSqliteDbContext>> logger, IDbContextFactory<EmployeeSqliteDbContext> contextFactory)
            : base(logger, contextFactory)
        {
        }

        public override IQueryable<EmployeeDepartment> GetSet(DbContext context) =>
            context.Set<EmployeeDepartment>().Include(e => e.Employees);
    }
}
