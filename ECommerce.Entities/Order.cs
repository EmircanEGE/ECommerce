﻿using ECommerce.Entities.Enums;

namespace ECommerce.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? CancelledReason { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.Pending;
        

        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
