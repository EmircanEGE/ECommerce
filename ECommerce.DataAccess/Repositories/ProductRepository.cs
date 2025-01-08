using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Entities;

namespace ECommerce.DataAccess.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ECommerceContext context) : base(context)
    {
    }
}