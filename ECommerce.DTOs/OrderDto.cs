using ECommerce.Entities.Enums;
using System.Text.Json.Serialization;

namespace ECommerce.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }
    }
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}
