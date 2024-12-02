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
        public async Task<IActionResult> CancelOrderByUser(int orderId, [FromBody] string? reason = null)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            await _orderService.CancelOrderByUser(userId, orderId, reason);
            return Ok("Order has been cancelled successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/orders/{orderId}/cancel")]
        public async Task<IActionResult> CancelOrderByAdmin(int orderId, [FromBody] string? reason = null)
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
        [HttpPut("admin/order-items/{orderItemId}")]
        public async Task<IActionResult> UpdateOrderItem(int orderItemId, [FromBody] int quantity)
        {
            await _orderService.UpdateOrderItem(orderItemId, quantity);
            return Ok("Order item has been updated, and total amount recalculated.");
        }
    }
}
