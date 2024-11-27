using ECommerce.DTOs;

namespace ECommerce.Business.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(int userId, CreateOrderDto createOrderDto);
        Task<List<OrderDto>> GetOrdersByUser(int userId);
        Task<OrderDto> GetOrderDetails(int orderId, int userId);
        Task DeleteOrder(int orderId, int userId);
    }
}
