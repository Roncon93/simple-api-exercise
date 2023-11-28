using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SimpleApiProject.Data
{
    /// <summary>
    /// The base repository with read and write capabilities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TDbContext">The type of the DB context.</typeparam>
    public abstract class BaseRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        private readonly ILogger<BaseRepository<TEntity, TDbContext>> logger;
        private readonly IDbContextFactory<TDbContext> contextFactory;

        public BaseRepository(ILogger<BaseRepository<TEntity, TDbContext>> logger, IDbContextFactory<TDbContext> contextFactory)
        {
            this.logger = logger;
            this.contextFactory = contextFactory;
        }

        /// <summary>
        /// Retrieves the entity set.
        /// </summary>
        /// <param name="context">The context to get the set of entities from.</param>
        /// <returns>The set of entities.</returns>
        public abstract IQueryable<TEntity> GetSet(DbContext context);

        /// <inheritdoc/>
        public virtual async Task CreateMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!entities.Any())
                {
                    return;
                }

                using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

                await context.AddRangeAsync(entities, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered exception while trying to create entities of type {Type}.", typeof(TEntity));

                throw;
            }
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> FindAll(CancellationToken cancellationToken = default)
        {
            try
            {
                using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

                return await GetSet(context).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while fetching list of entities for type {Type}", typeof(TEntity));

                throw;
            }
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity?> Find(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            try
            {
                using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

                return await GetSet(context).Where(expression).FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while fetching single entity of type {Type}", typeof(TEntity));

                throw;
            }
        }

        /// <inheritdoc/>
        public virtual async Task RemoveAll(CancellationToken cancellationToken = default)
        {
            try
            {
                using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

                var entities = await GetSet(context).ToListAsync();

                context.RemoveRange(entities);

                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered exception while removing entities of type {Type}.", typeof(TEntity));

                throw;
            }
        }
    }
}
