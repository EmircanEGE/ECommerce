namespace ECommerce.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

        
    }
    public class CreateOrderDto
        {
            public List<CreateOrderItemDto> OrderItems { get; set; }
        }
}
