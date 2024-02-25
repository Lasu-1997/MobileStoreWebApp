using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileStoreWebApp.Models;
using MobileStoreWebApp.Services;

namespace MobileStoreWebApp.Pages.Admin.Products
{
    public class DeleteModel : PageModel
    {
        private readonly ProductDbContext productDbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();
        public Product Product { get; set; } = new Product();
        public string errorMessage = string.Empty;
        public string successMessage = string.Empty;
        public DeleteModel(ProductDbContext _productDbContext, IWebHostEnvironment _webHostEnvironment)
        {
            productDbContext = _productDbContext;
            webHostEnvironment = _webHostEnvironment;
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            var product = productDbContext.Products.Find(id);

            if (product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            string imageFullPath = webHostEnvironment.WebRootPath + "/images/products/" + product.ImageFIleName;
            System.IO.File.Delete(imageFullPath);

            productDbContext.Products.Remove(product);
            productDbContext.SaveChanges();

            Response.Redirect("/Admin/Products/Index");
        }
    }
}
