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
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatusOrder(int id, [FromBody]UpdateOrderStatusDto newStatus)
        {
            await _orderService.UpdateOrderStatus(id, newStatus.Status);
            return Ok("Order status updated.");

        }
    }
}
