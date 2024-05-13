using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Common.DataLayer;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public Type ElementType => ((IQueryable<TEntity>) _dbSet).ElementType;

    public Expression Expression => ((IQueryable<TEntity>) _dbSet).Expression;

    public IQueryProvider Provider => ((IQueryable<TEntity>) _dbSet).Provider;

    public async Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _dbSet.Where(predicate).DeleteAsync();

    public async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TEntity>> updateFactory) =>
        await _dbSet.Where(predicate).UpdateAsync(updateFactory);

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> CreateManyAsync(IEnumerable<TEntity> entities)
    {
        var entitiesArray = entities as TEntity[] ?? entities.ToArray();
        await _dbSet.AddRangeAsync(entitiesArray);
        return entitiesArray;
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteMany(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task<TEntity> FindAsync(object[] keyValues) => await _dbSet.FindAsync(keyValues);

    public TEntity Update(TEntity entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public IEnumerable<TEntity> UpdateMany(IEnumerable<TEntity> entities)
    {
        var entitiesArray = entities as TEntity[] ?? entities.ToArray();
        _dbSet.UpdateRange(entitiesArray);
        return entitiesArray;
    }

    public IEnumerator<TEntity> GetEnumerator() => ((IQueryable<TEntity>) _dbSet).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}