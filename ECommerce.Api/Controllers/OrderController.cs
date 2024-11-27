using ECommerce.Business.Services.Interfaces;
using ECommerce.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var orderId = await _orderService.CreateOrder(userId, createOrderDto);
            return Ok(new { OrderId = orderId });
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var orders = await _orderService.GetOrdersByUser(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var order = await _orderService.GetOrderDetails(id, userId);
            return Ok(order);
        }
    }
}
