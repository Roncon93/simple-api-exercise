using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee, EmployeeSqliteDbContext>, IRepository<Employee>
    {
        public EmployeeRepository(ILogger<BaseRepository<Employee, EmployeeSqliteDbContext>> logger, IDbContextFactory<EmployeeSqliteDbContext> contextFactory)
            : base(logger, contextFactory)
        {
        }

        public override IQueryable<Employee> GetSet(DbContext context) =>
            context.Set<Employee>().Include(e => e.Company).Include(e => e.Department);
    }
}
