using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Entities;

namespace ECommerce.DataAccess.Repositories;

public class ReturnRequestRepository : Repository<ReturnRequest>, IReturnRequestRepository
{
    public ReturnRequestRepository(ECommerceContext context) : base(context)
    {
    }
}