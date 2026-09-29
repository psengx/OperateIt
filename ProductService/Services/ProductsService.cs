using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dto;
using ProductService.Grpc;
using ProductService.Models;

namespace ProductService.Services;

public class ProductsService
{
    private readonly ApplicationContext _context;
    public ProductsService(ApplicationContext context) => _context = context;

    public async Task<List<Product>> GetAllProducts()
    {
        return await _context.Products.AsNoTracking().ToListAsync();
    }
    public async Task<Product?> GetProductById(Guid id)
    {
        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateProduct(CreateProductRequest request)
    {
        Product product = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Stock = request.Stock,
            Price = request.Price
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        Product? product = await _context.Products
            .FirstOrDefaultAsync(p=>p.Id == id);
        if (product == null)
            return false;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<ProductReceptionResponse> AcceptReception(ReceptionRequest request)
    {
        Product? product = await _context.Products
            .FirstOrDefaultAsync(p=>p.Id == Guid.Parse(request.ProductId));
        if (product == null)
            return new()
            {
                Message = "Product not found"
            };
        
        product.Stock += request.Quantity;
        await _context.SaveChangesAsync();
        return new()
        {
            Success = true,
            Stock =  product.Stock,
            Message = $"[{product.Id.ToString().Substring(0,7)}...]    {product.Name} has been increased by {request.Quantity}"
        };
    }

}