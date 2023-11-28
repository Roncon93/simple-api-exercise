using SimpleApiProject.Data;
using SimpleApiProject.Models;

namespace SimpleApiProject.Services
{
    public class EmployeeDepartmentService : IEmployeeDepartmentService
    {
        private readonly IRepository<EmployeeDepartment> repository;

        public EmployeeDepartmentService(IRepository<EmployeeDepartment> repository)
        {
            this.repository = repository;
        }

        /// <inheritdoc/>
        public async Task RemoveAll(CancellationToken token = default) =>
            await repository.RemoveAll(token);
    }
}
