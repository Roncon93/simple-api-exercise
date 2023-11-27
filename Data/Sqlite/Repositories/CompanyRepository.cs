using SimpleApiProject.Models;
using System.Linq.Expressions;

namespace SimpleApiProject.Data.Sqlite.Repositories
{
    public class CompanyRepository : IRepository<Company>
    {
        public Task Create(Company entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Company>> FindAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Company?> FindOne(Expression<Func<Company, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
