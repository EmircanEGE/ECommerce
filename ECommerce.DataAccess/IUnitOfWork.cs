using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DataAccess;

public interface IUnitOfWork
{
    Task CommitAsync(IDbContextTransaction transaction);
    Task<IDbContextTransaction> CreateTransactionAsync();
    Task SaveChangeAsync();
}