using Microsoft.AspNetCore.Mvc;
using ProductService.Grpc;
using ReceptionService.Dto;
using ReceptionService.Services;

namespace ReceptionService.Controllers;

[Route("reception")]
[ApiController]
public class ReceptionsController : ControllerBase
{
    private readonly ReceptionsService _service;
    private readonly ProductGrpc.ProductGrpcClient _client;

    public ReceptionsController(ReceptionsService service, ProductGrpc.ProductGrpcClient client)
    {
        _service = service;
        _client = client;
    } 

    [HttpGet]
    public async Task<IActionResult> GetReceptions()
    {
        var response =await  _service.GetAllReceptions();
        return Ok(response);
    }

    [HttpPost("{id}/accept")]
    public async Task<IActionResult> AcceptReceptionById(Guid id)
    {
        var reception = await _service.GetReceptionById(id);
        if (reception == null)
            return NotFound();

        var reply = await _client.AcceptReceptionAsync(new()
        {
            ProductId = reception.ProductId.ToString(),
            Quantity = reception.ProductQuantity,
        });
        
        return Ok(reply);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateReception([FromBody] CreateReceptionRequest reception)
    { 
        await _service.CreateReception(reception);
        return Created();
    }
}