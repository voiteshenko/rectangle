using System.Linq.Expressions;

namespace Common.DataLayer;

public interface IRepository<TEntity> : IQueryable<TEntity>
    where TEntity : class
{
    Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> predicate);

    Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TEntity>> updateFactory);

    Task<TEntity> CreateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> CreateManyAsync(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);

    void DeleteMany(IEnumerable<TEntity> entities);

    Task<TEntity> FindAsync(object[] keyValues);

    TEntity Update(TEntity entity);

    IEnumerable<TEntity> UpdateMany(IEnumerable<TEntity> entities);
}