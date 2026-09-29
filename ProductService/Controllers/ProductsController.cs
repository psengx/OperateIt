using Microsoft.AspNetCore.Mvc;
using ProductService.Dto;

namespace ProductService.Controllers;

[Route("products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly Services.ProductsService _service;

    public ProductsController(Services.ProductsService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var response = await _service.GetAllProducts();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id)
    {
        var response = await _service.GetProductById(id);
        if  (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest product)
    {
        await  _service.CreateProduct(product);
        return Created();
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var deleted = await _service.DeleteProduct(id);
        return deleted ?  Ok() : NotFound();
    }
}