using ECommerce.Entities.Enums;

namespace ECommerce.DTOs;

public class DeliveryStatusDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public DeliveryStatus DeliveryStatus { get; set; }
}