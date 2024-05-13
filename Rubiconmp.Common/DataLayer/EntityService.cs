using System.Linq.Expressions;

namespace Common.DataLayer;

public class EntityService<TEntity> : IEntityService<TEntity>
    where TEntity : class
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IUnitOfWork UnitOfWork;

    public EntityService(IRepository<TEntity> repository, IUnitOfWork unitOfWork)
    {
        Repository = repository;
        UnitOfWork = unitOfWork;
    }

    public virtual async Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> predicate) =>
        await Repository.BatchDeleteAsync(predicate);

    public virtual async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TEntity>> updateFactory) =>
        await Repository.BatchUpdateAsync(predicate, updateFactory);

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        entity = await Repository.CreateAsync(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> CreateManyAsync(IEnumerable<TEntity> entities)
    {
        entities = await Repository.CreateManyAsync(entities);
        await UnitOfWork.SaveChangesAsync();
        return entities;
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        Repository.Delete(entity);
        await UnitOfWork.SaveChangesAsync();
    }

    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities)
    {
        Repository.DeleteMany(entities);
        await UnitOfWork.SaveChangesAsync();
    }

    public virtual async Task<TEntity> FindAsync(object[] keyValues) => await Repository.FindAsync(keyValues);

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity = Repository.Update(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> UpdateManyAsync(IEnumerable<TEntity> entities)
    {
        entities = Repository.UpdateMany(entities);
        await UnitOfWork.SaveChangesAsync();
        return entities;
    }

    public virtual void Dispose()
    {
        UnitOfWork?.Dispose();
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (UnitOfWork != null)
        {
            await UnitOfWork.DisposeAsync();
        }
    }
}