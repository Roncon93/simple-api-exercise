using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data.Sqlite.Contexts;
using System.Linq.Expressions;

namespace SimpleApiProject.Data
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly ILogger<BaseRepository<T>> logger;
        private readonly IDbContextFactory<CompanySqliteDbContext> contextFactory;

        public BaseRepository(ILogger<BaseRepository<T>> logger, IDbContextFactory<CompanySqliteDbContext> contextFactory)
        {
            this.logger = logger;
            this.contextFactory = contextFactory;
        }

        public async Task Create(T entity, CancellationToken cancellationToken = default)
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
                logger.LogError(ex, "Encountered exception while trying to create entity of type {Type}.", typeof(T));

                throw;
            }
        }

        public async Task<IEnumerable<T>> FindAll(CancellationToken cancellationToken = default)
        {
            using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

            return await context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T?> FindOne(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

            return await context.Set<T>().Where(expression).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
