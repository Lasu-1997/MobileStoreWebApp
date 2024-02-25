using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileStoreWebApp.Models;
using MobileStoreWebApp.Services;
using System.Reflection;

namespace MobileStoreWebApp.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly ProductDbContext productDbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();
        public string errorMessage = string.Empty;
        public string successMessage = string.Empty;
        public CreateModel(ProductDbContext _productDbContext, IWebHostEnvironment _webHostEnvironment)
        {
            productDbContext = _productDbContext;
            webHostEnvironment = _webHostEnvironment;
        }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            if(ProductDto.ImageFIleName == null)
            {
                ModelState.AddModelError("ProductDto.ImageFIleName","The image file is required!");
            }
            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fileds!";
                return;
            }

            //Save the image file
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            fileName += Path.GetExtension(ProductDto.ImageFIleName!.FileName);
            string imageFullPath = webHostEnvironment.WebRootPath + "/images/products/" + fileName;
            using(var stream = System.IO.File.Create(imageFullPath))
            {
                ProductDto.ImageFIleName.CopyTo(stream);
            }

            //Save the new product in the db
            Product product = new Product()
            {
                Name = ProductDto.Name,
                Brand = ProductDto.Brand,
                Category = ProductDto.Category,
                Price = ProductDto.Price,
                Description = ProductDto.Description,
                ImageFIleName = fileName,
                CreatedAt = DateTime.Now
            };

            productDbContext.Products.Add(product);
            productDbContext.SaveChanges();

            //Clear the form
            ProductDto.Name = "";
            ProductDto.Brand = "";
            ProductDto.Category = "";
            ProductDto.Price = 0;
            ProductDto.Description = "";
            ProductDto.ImageFIleName = null;

            ModelState.Clear();

            successMessage = "Product created successfullY!";

            Response.Redirect("/Admin/Products/Index");
        }
    }
}
