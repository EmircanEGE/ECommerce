using AutoMapper;
using ECommerce.Business.Services.Interfaces;
using ECommerce.DataAccess;
using ECommerce.DTOs;
using ECommerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly ECommerceContext _context;
        private readonly IMapper _mapper;

        public OrderService(ECommerceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateOrder(int userId, CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = createOrderDto.OrderItems.Sum(item => item.Quantity * item.UnitPrice),
                OrderItems = createOrderDto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                }).ToList(),
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<List<OrderDto>> GetOrdersByUser(int userId)
        {
            var orders = await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(xi => xi.Product)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderDetails(int orderId, int userId)
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude (xi => xi.Product)
                .FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);

            if (order == null)
                throw new Exception("Order not found or access denied.");
            
            return _mapper.Map<OrderDto>(order);
        }
    }
}
