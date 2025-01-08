using ECommerce.DataAccess.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly ECommerceContext _context;

    public UnitOfWork(ECommerceContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(IDbContextTransaction transaction)
    {
        try
        {
            await _context.SaveChangesAsync();
            transaction.CommitAsync();
        }
        catch (Exception e)
        {
            transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<IDbContextTransaction> CreateTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}