using SimpleApiProject.Models;

namespace SimpleApiProject.Services
{
    /// <summary>
    /// Interface to read and write <see cref="EmployeeDepartment"/> entities into the data store.
    /// </summary>
    public interface IEmployeeDepartmentService
    {
        /// <summary>
        /// Removes all the employee departments from the data store.
        /// </summary>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The operation's task.</returns>
        Task RemoveAll(CancellationToken token = default);
    }
}
