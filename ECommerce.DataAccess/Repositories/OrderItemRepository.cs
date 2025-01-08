using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Entities;

namespace ECommerce.DataAccess.Repositories;

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(ECommerceContext context) : base(context)
    {
    }
}