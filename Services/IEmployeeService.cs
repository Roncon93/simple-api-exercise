using SimpleApiProject.Models;

namespace SimpleApiProject.Services
{
    /// <summary>
    /// Interface to read and write <see cref="Employee"/> entities into the data store.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Inserts multiple employees into the data store.
        /// </summary>
        /// <param name="employees">The employees to insert.</param>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The operation's task.</returns>
        Task CreateMany(IEnumerable<Employee> employees, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single employee by their company ID and employee number.
        /// </summary>
        /// <param name="companyId">The ID of the company to look for.</param>
        /// <param name="employeeNumber">The employee number to look for.</param>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The employee if it was found.</returns>
        Task<Employee?> Find(int companyId, string employeeNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes all the employees from the data store.
        /// </summary>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The operation's task.</returns>
        Task RemoveAll(CancellationToken cancellationToken = default);
    }
}
