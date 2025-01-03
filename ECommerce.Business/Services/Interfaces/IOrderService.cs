using ECommerce.DTOs;
using ECommerce.Entities.Enums;

namespace ECommerce.Business.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(int userId, CreateOrderDto createOrderDto);
        Task<List<OrderDto>> GetOrdersByUser(int userId, OrderStatus? status);
        Task<List<OrderDto>> GetOrders(int? userId, OrderStatus? status);
        Task<OrderDto> GetOrderDetails(int orderId, int userId);
        Task DeleteOrder(int orderId, int userId);
        Task UpdateOrderStatus(int orderId, OrderStatus newStatus);
        Task CancelOrderByUser(int userId, int orderId, string reason);
        Task CancelOrderByAdmin(int orderId, string reason);
        Task UpdateTotalAmount(int orderId);
        Task UpdateOrderItem(int orderItemId, int quantity);
        Task<OrderReportDto> GenerateOrderReport(DateTime? startDate, DateTime? endDate);
        Task CreateReturnRequest(int userId, int orderId, string reason);

        Task<List<ReturnRequestDto>> GetReturnRequests(int? requestId, RequestStatus? status, PaginationDto pagination);
        Task ApproveReturnRequest(int requestId);
        Task RejectReturnRequest(int requestId, string rejectionReason);
        Task<DeliveryStatus> GetDeliveryStatus(int userId, int orderId);
        Task<List<DeliveryStatusDto>> GetDeliveryStatusAdmin(DeliveryStatus? status, DateTime? startDate,
            DateTime? endDate);
    }
}
