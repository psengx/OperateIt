using Grpc.Core;
using ProductService.Services;

namespace ProductService.Grpc;

public class ProductGrpcService : ProductGrpc.ProductGrpcBase
{
    private readonly ProductsService _service;
    public ProductGrpcService(ProductsService service)
    {
        _service = service;
    }
    public override async Task<ReceptionReply> AcceptReception(ReceptionRequest request, ServerCallContext context)
    {
        var result = await _service.AcceptReception(request);
        return new ReceptionReply()
        {
            Success = result.Success,
            NewStock = result.Stock,
            Message = result.Message
        };
    }
}
