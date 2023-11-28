using SimpleApiProject.Models;

namespace SimpleApiProject.Services
{
    /// <summary>
    /// Interface to read and write <see cref="Company"/> entities into the data store.
    /// </summary>
    public interface ICompanyService
    {
        /// <summary>
        /// Retrieves a single company by its ID.
        /// </summary>
        /// <param name="id">The ID of the company to look for.</param>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The company if it was found.</returns>
        Task<Company?> Find(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all the companies.
        /// </summary>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The list of all companies.</returns>
        Task<IEnumerable<Company>> FindAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes all the companies from the data store.
        /// </summary>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The operation's task.</returns>
        Task RemoveAll(CancellationToken cancellationToken = default);
    }
}
