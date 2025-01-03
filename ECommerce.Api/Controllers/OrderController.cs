using ECommerce.Business.Services.Interfaces;
using ECommerce.DTOs;
using ECommerce.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var order = await _orderService.CreateOrder(userId, createOrderDto);
            return Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/orders")]
        public async Task<IActionResult> GetAllOrders([FromQuery] int? userId, [FromQuery] OrderStatus? status = null)
        {
            var orders = await _orderService.GetOrders(userId, status);
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("user/orders")]
        public async Task<IActionResult> GetUserOrders([FromQuery] OrderStatus? status = null)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var orders = await _orderService.GetOrdersByUser(userId, status);
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var order = await _orderService.GetOrderDetails(id, userId);
            return Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId").Value);
                await _orderService.DeleteOrder(id, userId);
                return Ok("Order successfully deleted.");
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message});
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/orders/status")]
        public async Task<IActionResult> UpdateStatusOrder(UpdateOrderStatusDto dto)
        {
            await _orderService.UpdateOrderStatus(dto.OrderId, dto.NewStatus);
            return Ok("Order status updated successfully.");

        }

        [Authorize]
        [HttpPut("user/orders/{orderId}/cancel")]
        public async Task<IActionResult> CancelOrderByUser(int orderId, [FromBody] string reason)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            await _orderService.CancelOrderByUser(userId, orderId, reason);
            return Ok("Order has been cancelled successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/orders/{orderId}/cancel")]
        public async Task<IActionResult> CancelOrderByAdmin(int orderId, [FromBody] string reason)
        {
            await _orderService.CancelOrderByAdmin(orderId, reason);
            return Ok("Order has been cancelled successfully.");
        }

        [Authorize]
        [HttpPut("user/orders/{orderId}/update-total")]
        public async Task<IActionResult> UpdateOrderTotal(int orderId)
        {
            await _orderService.UpdateTotalAmount(orderId);
            return Ok("Order total amount has been updated.");
        }

        [Authorize]
        [HttpPut("user/order-items/{orderItemId}")]
        public async Task<IActionResult> UpdateOrderItem(int orderItemId, [FromBody] int quantity)
        {
            await _orderService.UpdateOrderItem(orderItemId, quantity);
            return Ok("Order item has been updated, and total amount recalculated.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/orders/report")]
        public async Task<IActionResult> GetOrderReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var report = await _orderService.GenerateOrderReport(startDate, endDate);
            return Ok(report);
        }

        [Authorize]
        [HttpPost("user/orders/{orderId}/return")]
        public async Task<IActionResult> CreateReturnOrder(int orderId, [FromBody] string reason)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            await _orderService.CreateReturnRequest(userId, orderId, reason);
            return Ok("Return request has been created successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/orders/return")]
        public async Task<IActionResult> GetReturnOrders([FromQuery] PaginationDto pagination, int? requestId, [FromQuery] RequestStatus? status = null)
        {
            var orders = await _orderService.GetReturnRequests(requestId, status, pagination);
            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/orders/return/{requestId}/approve")]
        public async Task<IActionResult> ApproveReturnRequest(int requestId)
        {
            await _orderService.ApproveReturnRequest(requestId);
            return Ok("Return request approved successfully.");
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/orders/return/{requestId}/reject")]
        public async Task<IActionResult> RejectReturnRequest(int requestId, [FromBody] string reason)
        {
            await _orderService.RejectReturnRequest(requestId, reason);
            return Ok("Return request rejected successfully.");
        }
        
        [Authorize]
        [HttpGet("user/orders/delivery-status")]
        public async Task<IActionResult> GetDeliveryStatus(int orderId)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var orders = await _orderService.GetDeliveryStatus(userId, orderId);
            return Ok(orders);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/orders/delivery-status")]
        public async Task<IActionResult> GetDeliveryStatusAdmin(DeliveryStatus? status, DateTime? startDate, DateTime? endDate)
        {
            var orders = await _orderService.GetDeliveryStatusAdmin(status, startDate, endDate);
            return Ok(orders);
        }
    }
}
