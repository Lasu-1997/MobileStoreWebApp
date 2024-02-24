using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileStoreWebApp.Models;
using MobileStoreWebApp.Services;

namespace MobileStoreWebApp.Pages.Admin.Products
{
    public class IndexModel : PageModel
    {
        private readonly ProductDbContext productDbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public List<Product> Products { get; set; } = new List<Product>();
        public IndexModel(ProductDbContext _productDbContext, IWebHostEnvironment _webHostEnvironment)
        {
            productDbContext = _productDbContext;
            webHostEnvironment = _webHostEnvironment;
        }
        public void OnGet()
        {
            Products = productDbContext.Products.OrderByDescending(p => p.Id).ToList();
        }
    }
}
