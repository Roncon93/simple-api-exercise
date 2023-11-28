using SimpleApiProject.Data;
using SimpleApiProject.Models;

namespace SimpleApiProject.Services
{
    public interface ICompanyService
    {
        Task<Company?> Find(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Company>>FindMany(CancellationToken cancellationToken = default);

        Task RemoveMany(CancellationToken cancellationToken = default);
    }

    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> repository;

        public CompanyService(IRepository<Company> repository)
        {
            this.repository = repository;
        }

        public async Task<Company?> Find(int id, CancellationToken cancellationToken = default) =>
            await repository.Find(c => c.Id == id, cancellationToken);

        public async Task<IEnumerable<Company>> FindMany(CancellationToken cancellationToken = default) =>
            await repository.FindMany(cancellationToken);

        public async Task RemoveMany(CancellationToken cancellationToken = default) =>
            await repository.RemoveMany(cancellationToken);
    }
}
