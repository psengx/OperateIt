using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
}

public static class TestData
{
    public static List<Product> Insert()
    {
            var products = new List<Product>();
            var random = new Random();
            var names = new[]
            {
                "Ноутбук", "Смартфон", "Планшет", "Наушники", "Клавиатура",
                "Мышь", "Монитор", "Принтер", "Сканер", "Веб-камера",
                "Колонки", "Микрофон", "Роутер", "Модем", "Флешка",
                "Внешний диск", "Карта памяти", "Видеокарта", "Процессор", "Материнская плата",
                "Блок питания", "Корпус", "Кулер", "Термопаста", "ИБП",
                "Кабель HDMI", "Кабель USB", "Переходник", "Док-станция", "Гарнитура"
            };

            for (int i = 0; i < 30; i++)
            {
                products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Name = names[i],
                    Price = Math.Round((decimal)(random.NextDouble() * 99000 + 1000), 2),
                    Stock = random.Next(0, 500)
                });
            }
            return products;
    }
}