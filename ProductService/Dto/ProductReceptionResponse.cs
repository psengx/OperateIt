namespace ProductService.Dto;

public class ProductReceptionResponse
{
    public bool Success { get; set; } = false;
    public int Stock { get; set; }
    public string Message { get; set; } =  string.Empty;
}