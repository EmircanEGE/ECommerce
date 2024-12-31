using ECommerce.Entities.Enums;

namespace ECommerce.Entities;

public class ReturnRequest
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Reason { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    
    public virtual Order Order { get; set; }
}