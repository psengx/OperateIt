namespace ReceptionService.Dto;

public class CreateReceptionRequest
{
    public Guid ProductId { get; set; }
    public int ProductQuantity { get; set; }
}