using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Models;
using System.Linq.Expressions;

namespace SimpleApiProject.Data
{
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

        public abstract IQueryable<TEntity> GetSet(DbContext context);

        public virtual async Task Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity is null)
                {
                    logger.LogError("Attempting to insert null entity.");

                    throw new ArgumentNullException(nameof(entity));
                }

                using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

                await context.AddAsync(entity, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered exception while trying to create entity of type {Type}.", typeof(TEntity));

                throw;
            }
        }

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

        public virtual async Task<IEnumerable<TEntity>> FindMany(CancellationToken cancellationToken = default)
        {
            using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

            return await GetSet(context).ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity?> Find(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

            return await GetSet(context).Where(expression).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task RemoveMany(CancellationToken cancellationToken = default)
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
