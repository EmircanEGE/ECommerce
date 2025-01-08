using System.Linq.Expressions;

namespace ECommerce.DataAccess.Repositories.Interfaces;

public interface IRepository<T>
{
    Task InsertAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    IQueryable<T> Get(Expression<Func<T, bool>> expression);
}