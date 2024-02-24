using Microsoft.EntityFrameworkCore;

namespace MobileStoreWebApp.Services
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
