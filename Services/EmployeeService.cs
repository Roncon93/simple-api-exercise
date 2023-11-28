using SimpleApiProject.Data;
using SimpleApiProject.Models;

namespace SimpleApiProject.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> repository;

        public EmployeeService(IRepository<Employee> repository)
        {
            this.repository = repository;
        }

        /// <inheritdoc/>
        public async Task CreateMany(IEnumerable<Employee> employees, CancellationToken cancellationToken = default) =>
            await repository.CreateMany(employees, cancellationToken);

        /// <inheritdoc/>
        public async Task<Employee?> Find(int companyId, string employeeNumber, CancellationToken cancellationToken = default)
        {
            var employee = await repository.Find(e => e.CompanyId == companyId && e.EmployeeNumber == employeeNumber, cancellationToken);

            if (employee is not null)
            {
                employee.Managers = await FindManager(companyId, employee.ManagerEmployeeNumber, [], cancellationToken);
            }

            return employee;
        }

        /// <inheritdoc/>
        public async Task RemoveAll(CancellationToken cancellationToken = default) =>
            await repository.RemoveAll(cancellationToken);

        /// <summary>
        /// Recusively loads the list of an employee's managers starting with
        /// their most immediate manager.
        /// </summary>
        /// <param name="companyId">The company ID of the employee and their managers.</param>
        /// <param name="managerEmployeeNumber">The employee number of the employee's manager.</param>
        /// <param name="managers">The list of managers to add to.</param>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The list of managers found.</returns>
        private async Task<List<Employee>> FindManager(int companyId, string managerEmployeeNumber, List<Employee> managers, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(managerEmployeeNumber))
            {
                return managers;
            }

            var manager = await repository.Find(e =>
                e.CompanyId == companyId &&
                e.EmployeeNumber == managerEmployeeNumber, cancellationToken);

            if (manager is not null)
            {
                // Add to the list before fetching the next manager
                // to ensure the most senior manager is last in the list
                managers.Add(manager);

                // Find the manager's manager if they have one
                await FindManager(companyId, manager.ManagerEmployeeNumber, managers, cancellationToken);
            }

            return managers;
        }
    }
}
