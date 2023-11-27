using System.Linq.Expressions;

namespace SimpleApiProject.Data
{
    public interface IRepository<T>
    {
        Task Create(T entity, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> FindAll(CancellationToken cancellationToken = default);

        Task<T?> FindOne(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    }
}
