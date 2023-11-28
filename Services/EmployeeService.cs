using SimpleApiProject.Data;
using SimpleApiProject.Models;
using System.ComponentModel.Design;
using System.Threading;

namespace SimpleApiProject.Services
{
    public interface IEmployeeService
    {
        Task CreateMany(IEnumerable<Employee> employees, CancellationToken cancellationToken = default);

        Task<Employee?> Find(int companyId, string employeeNumber, CancellationToken cancellationToken = default);

        Task RemoveMany(CancellationToken cancellationToken = default);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> repository;

        public EmployeeService(IRepository<Employee> repository)
        {
            this.repository = repository;
        }

        public async Task CreateMany(IEnumerable<Employee> employees, CancellationToken cancellationToken = default) =>
            await repository.CreateMany(employees, cancellationToken);

        public async Task<Employee?> Find(int companyId, string employeeNumber, CancellationToken cancellationToken = default)
        {
            var employee = await repository.Find(e => e.CompanyId == companyId && e.EmployeeNumber == employeeNumber, cancellationToken);

            if (employee is not null)
            {
                employee.Managers = await FindManager(companyId, employee.ManagerEmployeeNumber, [], cancellationToken);
            }

            return employee;
        }

        public async Task RemoveMany(CancellationToken cancellationToken = default) =>
            await repository.RemoveMany(cancellationToken);

        private async Task<List<Employee>> FindManager(int companyId, string managerEmployeeNumber, List<Employee> managers, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(managerEmployeeNumber))
            {
                return managers;
            }

            var manager = await repository.Find(e => e.CompanyId == companyId && e.EmployeeNumber == managerEmployeeNumber, cancellationToken);

            if (manager is not null)
            {
                managers.Add(manager);

                await FindManager(companyId, manager.ManagerEmployeeNumber, managers, cancellationToken);
            }

            return managers;
        }
    }
}
