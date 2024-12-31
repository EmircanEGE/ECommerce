using ECommerce.Entities.Enums;

namespace ECommerce.DTOs;

public class ReturnRequestDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Reason { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime RequestedAt { get; set; } 
}