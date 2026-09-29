using Microsoft.EntityFrameworkCore;
using ReceptionService.Models;

namespace ReceptionService.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Reception> Receptions { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
}