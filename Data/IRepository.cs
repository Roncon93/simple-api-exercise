using System.Linq.Expressions;

namespace SimpleApiProject.Data
{
    /// <summary>
    /// Repository interface to define how to read and write to the data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to store.</typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Creates many entities in a bulk transaction.
        /// </summary>
        /// <param name="entities">The entities to be inserted to the data store.</param>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The operation's task.</returns>
        Task CreateMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the single entity that matches the search expression.
        /// </summary>
        /// <param name="expression">The search criteria.</param>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The entity if it was found.</returns>
        Task<TEntity?> Find(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all entities.
        /// </summary>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The list of entities found.</returns>
        Task<IEnumerable<TEntity>> FindAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes all entities in a bulk transaction. 
        /// </summary>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The operation's task.</returns>
        Task RemoveAll(CancellationToken cancellationToken = default);
    }
}
