using System.Linq.Expressions;

namespace SimpleApiProject.Data
{
    public interface IRepository<TEntity>
    {
        Task Create(TEntity entity, CancellationToken cancellationToken = default);

        Task CreateMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity?> Find(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> FindMany(CancellationToken cancellationToken = default);

        Task RemoveMany(CancellationToken cancellationToken = default);
    }
}
