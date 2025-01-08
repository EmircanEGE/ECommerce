using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Entities;

namespace ECommerce.DataAccess.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ECommerceContext context) : base(context)
    {
    }
}