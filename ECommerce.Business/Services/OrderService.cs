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

        public async Task<OrderDto> CreateOrder(int userId, CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in createOrderDto.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new Exception("Product not found!");

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            order.TotalAmount = order.OrderItems.Sum(x => x.Quantity * x.UnitPrice);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return _mapper.Map<OrderDto>(order);
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

        public async Task DeleteOrder(int orderId, int userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);
            if (order == null)
                throw new Exception("Order not found or access denied.");
            
            _context.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
