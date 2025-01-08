using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Entities;

namespace ECommerce.DataAccess.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ECommerceContext context) : base(context)
    {
    }
}