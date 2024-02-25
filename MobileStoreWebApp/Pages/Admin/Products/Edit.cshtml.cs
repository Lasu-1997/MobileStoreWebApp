using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileStoreWebApp.Models;
using MobileStoreWebApp.Services;

namespace MobileStoreWebApp.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly ProductDbContext productDbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();
        public Product Product { get; set; } = new Product();
        public string errorMessage = string.Empty;
        public string successMessage = string.Empty;
        public EditModel(ProductDbContext _productDbContext, IWebHostEnvironment _webHostEnvironment)
        {
            productDbContext = _productDbContext;
            webHostEnvironment = _webHostEnvironment;
        }
        public void OnGet(int? id)
        {
            if(id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            var product = productDbContext.Products.Find(id);

            if(product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }
            ProductDto.Name = product.Name;
            ProductDto.Brand = product.Brand;
            ProductDto.Category = product.Category;
            ProductDto.Price = product.Price;
            ProductDto.Description = product.Description;

            Product = product;
        }
        public void OnPost(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fileds!";
                return;
            }

            var product = productDbContext.Products.Find(id);

            if(product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            //Update the image file if we have a new image file
            string? newFileName = product.ImageFIleName;
            if(ProductDto.ImageFIleName != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(ProductDto.ImageFIleName!.FileName);
                string imageFullPath = webHostEnvironment.WebRootPath + "/images/products/" + newFileName;
                using(var stream = System.IO.File.Create(imageFullPath))
                {
                    ProductDto.ImageFIleName.CopyTo(stream);
                }

                //Delete the old image
                string oldImagefullPath = webHostEnvironment.WebRootPath + "/images/products/" + product.ImageFIleName;
                System.IO.File.Delete(oldImagefullPath);
            }

            //Update the product in the db
            product.Name = ProductDto.Name;
            product.Brand = ProductDto.Brand;
            product.Category = ProductDto.Category;
            product.Price = ProductDto.Price;
            product.Description = ProductDto.Description;
            product.ImageFIleName = newFileName;

            productDbContext.SaveChanges();

            Product = product;

            successMessage = "Product updated successfully!";
        }
    }
}
