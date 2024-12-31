namespace ECommerce.DTOs;

public class OrderReportDto
{
    public decimal TotalSales { get; set; }
    public int TotalOrders { get; set; }
    public int CancelledOrders { get; set; }
    public int CompletedOrders { get; set; }
    public List<TopProductDto> TopProducts { get; set; }
}