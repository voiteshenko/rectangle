using System.Linq.Expressions;

namespace Common.DataLayer;

public interface IEntityService<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : class
{
    Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> predicate);

    Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TEntity>> updateFactory);

    Task<TEntity> CreateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> CreateManyAsync(IEnumerable<TEntity> entities);

    Task DeleteAsync(TEntity entity);

    Task DeleteManyAsync(IEnumerable<TEntity> entities);

    Task<TEntity> FindAsync(object[] keyValues);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> UpdateManyAsync(IEnumerable<TEntity> entities);
}