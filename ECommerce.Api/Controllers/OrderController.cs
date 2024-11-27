using ECommerce.Business.Services.Interfaces;
using ECommerce.DTOs;
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

        [Authorize(Roles = ("Admin,User"))]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var order = await _orderService.CreateOrder(userId, createOrderDto);
            return Ok(order);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var orders = await _orderService.GetOrdersByUser(userId);
            return Ok(orders);
        }

        [Authorize(Roles = "Admin,User")]
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
    }
}
