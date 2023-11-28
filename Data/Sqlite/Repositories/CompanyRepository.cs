using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Repositories
{
    public class CompanyRepository : BaseRepository<Company, CompanySqliteDbContext>, IRepository<Company>
    {
        public CompanyRepository(ILogger<BaseRepository<Company, CompanySqliteDbContext>> logger, IDbContextFactory<CompanySqliteDbContext> contextFactory)
            : base(logger, contextFactory)
        {
        }

        public override IQueryable<Company> GetSet(DbContext context) =>
            context.Set<Company>().Include(c => c.Employees);
    }
}
