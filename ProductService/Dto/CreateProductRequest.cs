namespace ProductService.Dto;

public class CreateProductRequest
{
    public string? Name { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
}