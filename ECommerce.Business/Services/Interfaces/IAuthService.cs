using ECommerce.DTOs;
using ECommerce.Entities;

namespace ECommerce.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(UserRegisterDto userRegisterDto);
        Task<string> Login(UserLoginDto userLoginDto);

    }
}
