using HelloRabbitMq.Watermark.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloRabbitMq.Watermark.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
