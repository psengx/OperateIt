using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Grpc;
using ProductService.Services;

namespace ProductService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<ProductsService>();
        builder.Services.AddGrpc();
        
        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            db.Database.Migrate();
            if (!db.Products.Any())
            {
                db.Products.AddRange(TestData.Insert());
                db.SaveChanges();
            }
        };
        
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapGrpcService<ProductGrpcService>();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        
        app.Run();
    }
}