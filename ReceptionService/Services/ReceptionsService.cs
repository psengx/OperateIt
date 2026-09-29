using Microsoft.EntityFrameworkCore;
using ReceptionService.Data;
using ReceptionService.Dto;
using ReceptionService.Models;

namespace ReceptionService.Services;

public class ReceptionsService
{
    private readonly ApplicationContext _context;
    
    public ReceptionsService(ApplicationContext context) => _context = context;

    public async Task<List<Reception>> GetAllReceptions()
    {
        return await _context.Receptions.AsNoTracking().ToListAsync();
    }
    public async Task<Reception?> GetReceptionById(Guid id)
    {
        var reception = await _context.Receptions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (reception == null) return null;
        
        _context.Receptions.Remove(reception);
        await _context.SaveChangesAsync();
        
        return reception;
    }

    public async Task<Reception> CreateReception(CreateReceptionRequest request)
    {
        Reception reception = new()
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            ProductQuantity = request.ProductQuantity,
            DeliveredAt = DateTime.UtcNow
        };
        _context.Receptions.Add(reception);
        await _context.SaveChangesAsync();
        return reception;
    }
}