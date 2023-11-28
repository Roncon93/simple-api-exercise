using SimpleApiProject.Data;
using SimpleApiProject.Models;

namespace SimpleApiProject.Services
{
    public interface IEmployeeDepartmentService
    {
        Task CreateMany(IEnumerable<EmployeeDepartment> departments, CancellationToken token = default);

        Task RemoveMany(CancellationToken token = default);
    }

    public class EmployeeDepartmentService : IEmployeeDepartmentService
    {
        private readonly IRepository<EmployeeDepartment> repository;

        public EmployeeDepartmentService(IRepository<EmployeeDepartment> repository)
        {
            this.repository = repository;
        }

        public async Task CreateMany(IEnumerable<EmployeeDepartment> departments, CancellationToken token = default) =>
            await repository.CreateMany(departments, token);

        public async Task RemoveMany(CancellationToken token = default) =>
            await repository.RemoveMany(token);
    }
}
