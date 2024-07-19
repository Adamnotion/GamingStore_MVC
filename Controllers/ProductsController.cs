using GamingStore.Models;
using GamingStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace GamingStore.Controllers
{
    public class ProductsController : Controller
    {
        public readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image is required");
            }
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);
            string imageFullPath = environment.WebRootPath + "/assets/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };


            context.Products.Add(product);
            context.SaveChanges();


            return RedirectToAction("Index", "Products");
        }

        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

            return View(productDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductDto ProductDto)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

                return View(ProductDto);
            }

            string newFileName = product.ImageFileName;
            if (ProductDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(ProductDto.ImageFile.FileName);
            

            string imageFullPath = environment.WebRootPath + "/assets/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                ProductDto.ImageFile.CopyTo(stream);
            }
            string oldImageFullPath = environment.WebRootPath + "/assets/" + product.ImageFileName;
            System.IO.File.Delete(oldImageFullPath);
            }

        product.Name=ProductDto.Name;
        product.Brand=ProductDto.Brand;
        product.Category=ProductDto.Category;
        product.Price=ProductDto.Price;
        product.Description=ProductDto.Description;
        product.ImageFileName=newFileName;

        context.SaveChanges();
        return RedirectToAction("Index" , "Products");

        }
        public IActionResult Delete(int id )
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            string imageFullPath = environment.WebRootPath + "/assets/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges(true);
            return RedirectToAction("Index", "Products");
        }

    }
}
