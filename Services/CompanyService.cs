using SimpleApiProject.Data;
using SimpleApiProject.Models;

namespace SimpleApiProject.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> repository;

        public CompanyService(IRepository<Company> repository)
        {
            this.repository = repository;
        }

        public async Task<Company?> Find(int id, CancellationToken cancellationToken = default) =>
            await repository.Find(c => c.Id == id, cancellationToken);

        public async Task<IEnumerable<Company>> FindAll(CancellationToken cancellationToken = default) =>
            await repository.FindAll(cancellationToken);

        public async Task RemoveAll(CancellationToken cancellationToken = default) =>
            await repository.RemoveAll(cancellationToken);
    }
}
