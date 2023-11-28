using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts;
using SimpleApiProject.Models;

namespace SimpleApiProject.Data.Sqlite.Repositories
{
    /// <summary>
    /// A Sqlite implementation of a data store repository for the <see cref="Company"/> entity.
    /// </summary>
    public class CompanyRepository : BaseRepository<Company, CompanySqliteDbContext>, IRepository<Company>
    {
        public CompanyRepository(ILogger<BaseRepository<Company, CompanySqliteDbContext>> logger, IDbContextFactory<CompanySqliteDbContext> contextFactory)
            : base(logger, contextFactory)
        {
        }

        /// <inheritdoc/>
        public override IQueryable<Company> GetSet(DbContext context) =>
            context.Set<Company>().Include(c => c.Employees);
    }
}
