using AutoMapper;
using ECommerce.Business.Services.Interfaces;
using ECommerce.DataAccess;
using ECommerce.DTOs;
using ECommerce.Entities;
using ECommerce.Entities.Enums;
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

        public async Task<List<OrderDto>> GetOrdersByUser(int userId, OrderStatus? status)
        {
            var query = _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(xi => xi.Product)
                .Where(x => x.UserId == userId);
            
            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);
            
            var orders = await query.ToListAsync();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<List<OrderDto>> GetOrders(int? userId, OrderStatus? status)
        {
            var query = _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(xi => xi.Product)
                .AsQueryable();
            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);
            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId.Value);
            
            var orders = await query.ToListAsync();
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

        public async Task UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
                throw new Exception("Order not found.");
            if (order.Status == OrderStatus.Completed)
                throw new Exception("Completed orders cannot be updated.");
            if (order.Status == OrderStatus.Cancelled)
                throw new Exception("Cancelled orders cannot be updated.");
            if (order.Status != OrderStatus.Approved &&  newStatus == OrderStatus.Shipped)
                throw new Exception("Orders must be approved before they can be marked as 'Shipped'.");

            order.Status = newStatus;
            await _context.SaveChangesAsync();
        }

        public async Task CancelOrderByUser(int userId, int orderId, string reason)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);
            if (order == null)
                throw new Exception("Order not found or access denied.");
            if(order.Status != OrderStatus.Pending)
                throw new Exception("Only pending orders can be cancelled.");
            if(string.IsNullOrWhiteSpace(reason))
                throw new Exception("Cancellation reason must be provided.");
            
            order.Status = OrderStatus.Cancelled;
            order.CancelledReason = reason;
            
            await _context.SaveChangesAsync();
        }

        public async Task CancelOrderByAdmin(int orderId, string reason)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
                throw new Exception("Order not found or access denied.");
            if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Shipped)
                throw new Exception("Shipped or completed orders cannot be cancelled.");
            if (string.IsNullOrWhiteSpace(reason))
                throw new Exception("Cancellation reason must be provided.");
            
            order.Status = OrderStatus.Cancelled;
            order.CancelledReason = reason;
            
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTotalAmount(int orderId)
        {
            var order = await _context.Orders
                .Include(x=> x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
                throw new Exception("Order not found.");
            if(!order.OrderItems.Any())
                throw new Exception("Order has no items.");
            
            order.TotalAmount = order.OrderItems.Sum(x => x.Quantity * x.UnitPrice);
            
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderItem(int orderItemId, int quantity)
        {
            var orderItem = await _context.OrderItems
                .Include(x=> x.Order)
                .FirstOrDefaultAsync(x => x.Id == orderItemId);
            
            if (orderItem == null)
                throw new Exception("Order item not found.");
            if(quantity <= 0)
                throw new Exception("Quantity cannot be less or equal to zero.");
            
            orderItem.Quantity = quantity;
            await _context.SaveChangesAsync();
        }

        public async Task<OrderReportDto> GenerateOrderReport(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(xi => xi.Product)
                .AsQueryable();
            
            if (startDate.HasValue)
                query = query.Where(x => x.OrderDate >= startDate);
            
            if (endDate.HasValue)
                query = query.Where(x => x.OrderDate <= endDate);
            
            var totalSales = await query.SumAsync(o => o.TotalAmount);
            var totalOrders = await query.CountAsync();
            var CancelledOrders = await query.CountAsync(x => x.Status == OrderStatus.Cancelled);
            var CompletedOrders = await query.CountAsync(x => x.Status == OrderStatus.Completed);
            
            var topProducts = await query
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Name)
                .Select(g => new TopProductDto
                {
                    ProductName = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(tp => tp.QuantitySold)
                .Take(5)
                .ToListAsync();

            return new OrderReportDto
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                CancelledOrders = CancelledOrders,
                CompletedOrders = CompletedOrders,
                TopProducts = topProducts
            };
        }

        public async Task CreateReturnRequest(int userId, int orderId, string reason)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);
            if (order == null) 
                throw new Exception("Order not found or access denied.");
            
            if (order.Status != OrderStatus.Completed)
                throw new Exception("Return request can only be created for completed orders.");

            var existingRequest = await _context.ReturnRequests.AnyAsync(x => x.OrderId == orderId && x.Status == RequestStatus.Pending);
            if (existingRequest)
                throw new Exception("There is already a pending request for this order.");

            var returnRequest = new ReturnRequest
            {
                OrderId = orderId,
                Reason = reason
            };
            
            await _context.ReturnRequests.AddAsync(returnRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReturnRequestDto>> GetReturnRequests(int? requestId, RequestStatus? status, PaginationDto pagination)
        {
            var query = _context.ReturnRequests
                .Include(r => r.Order)
                .AsQueryable();
        
            if (requestId.HasValue)
                query = query.Where(r => r.Id == requestId);
            
            if (status.HasValue)
                query = query.Where(r => r.Status == status);
            
            var returnRequests = await query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
            
            return _mapper.Map<List<ReturnRequestDto>>(returnRequests);
        }

        public async Task ApproveReturnRequest(int requestId)
        {
            var returnRequest = await _context.ReturnRequests.FirstOrDefaultAsync(x => x.Id == requestId);
            
            if (returnRequest == null)
                throw new Exception("Return request not found!");
            
            if (returnRequest.Status != RequestStatus.Pending)
                throw new Exception("Only pending requests can be approved.");
            
            returnRequest.Status = (RequestStatus.Approved);
            await _context.SaveChangesAsync();
        }

        public async Task RejectReturnRequest(int requestId, string rejectionReason)
        {
            var returnRequest = await _context.ReturnRequests.FirstOrDefaultAsync(x => x.Id == requestId);
            
            if (returnRequest == null)
                throw new Exception("Return request not found!");
            
            if (returnRequest.Status != RequestStatus.Pending)
                throw new Exception("Only pending requests can be rejected.");
            
            returnRequest.Status = RequestStatus.Rejected;
            returnRequest.Reason = rejectionReason;
            await _context.SaveChangesAsync();
        }

        public async Task<DeliveryStatus> GetDeliveryStatus(int userId, int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == orderId);
            if (order == null)
                throw new Exception("Order not found or access denied.");

            return order.DeliveryStatus;
        }

        public async Task<List<DeliveryStatusDto>> GetDeliveryStatusAdmin(DeliveryStatus? status, DateTime? startDate,
            DateTime? endDate)
        {
            var query = _context.Orders.AsQueryable();
            
            if (status.HasValue)
                query = query.Where(x => x.DeliveryStatus == status.Value);
            
            if (startDate.HasValue)
                query = query.Where(x => x.OrderDate >= startDate);
            
            if (endDate.HasValue)
                query = query.Where(x => x.OrderDate <= endDate);
            
            var orders = await query.ToListAsync();
            return _mapper.Map<List<DeliveryStatusDto>>(orders);
        }
    }
}
