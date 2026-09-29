namespace ReceptionService.Models;

public class Reception
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int ProductQuantity { get; set; }
    public DateTime DeliveredAt { get; set; }
}