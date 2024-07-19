using GamingStore.Models;
using GamingStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace GamingStore.Controllers
{
    public class StoreController : Controller
    {
        public readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public StoreController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            // Fetch products from the database
            IEnumerable<Product> products = context.Products.ToList(); // Ensure to include ToList() to materialize the query
            return View(products);
        }
        public IActionResult Details(int id)
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
        public IActionResult Details(int id, ProductDto ProductDto)
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
               

                return View(ProductDto);
            }

            

            product.Name = ProductDto.Name;
            product.Brand = ProductDto.Brand;
            product.Category = ProductDto.Category;
            product.Price = ProductDto.Price;
            product.Description = ProductDto.Description;
            

            context.SaveChanges();
            return RedirectToAction("Index", "Products");

        }




    }
}
