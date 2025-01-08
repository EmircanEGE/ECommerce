using System.Linq.Expressions;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;

    public Repository(ECommerceContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public IQueryable<T> Get(Expression<Func<T, bool>> expression)
    {
        return  _dbSet.AsNoTracking().Where(expression);
    }
}