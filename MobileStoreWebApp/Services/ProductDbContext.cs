using Microsoft.EntityFrameworkCore;
using MobileStoreWebApp.Models;

namespace MobileStoreWebApp.Services
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
