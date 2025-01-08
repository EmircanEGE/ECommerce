using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DataAccess.Migrations;

public interface IUnitOfWork
{
    Task CommitAsync(IDbContextTransaction transaction);
    Task<IDbContextTransaction> CreateTransactionAsync();
    Task SaveChangeAsync();
}