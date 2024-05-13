using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.DataLayer;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public IDbContextTransaction Transaction { get; private set; }

    public async Task<IDbContextTransaction> CreateTransactionAsync()
    {
        if (Transaction != null)
        {
            throw new InvalidOperationException("There is already a transaction");
        }

        Transaction = await _context.Database.BeginTransactionAsync();
        return Transaction;
    }

    public async Task CommitTransactionAsync()
    {
        if (Transaction == null)
        {
            throw new InvalidOperationException("There is no active transaction");
        }

        try
        {
            await SaveChangesAsync();
            await Transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
        }
        finally
        {
            if (Transaction != null)
            {
                await Transaction.DisposeAsync();
                Transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (Transaction == null)
        {
            throw new InvalidOperationException("There is no active transaction");
        }

        try
        {
            await Transaction.RollbackAsync();
        }
        finally
        {
            await Transaction.DisposeAsync();
            Transaction = null;
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public virtual void Dispose()
    {
        _context?.Dispose();
        Transaction?.Dispose();
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }

        if (Transaction != null)
        {
            await Transaction.DisposeAsync();
        }
    }
}