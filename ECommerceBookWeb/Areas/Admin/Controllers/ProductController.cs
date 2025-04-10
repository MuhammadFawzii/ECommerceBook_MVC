using ECommerceBook.DataAccess.Repository;
using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerceBook.Models.ViewModels;
using NuGet.Protocol.Plugins;
namespace ECommerceBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(IUnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment) {
            unitOfWork = _unitOfWork;
            webHostEnvironment= _webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = unitOfWork.ProductRepository.GetAll(includeProperties:"Category");
            return View(products);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = unitOfWork.CategoryRepository.GetAll()
                    ?.Select(i => new SelectListItem(i.Name, i.Id.ToString()))
                    ?? Enumerable.Empty<SelectListItem>(); // Fallback to empty list if null            
            
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = categoryList
            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath=webHostEnvironment.WebRootPath;
                if (file!=null)
                {
                   productVM.Product.ImageUrl= ProcessUploadedImage(file);

                }
                unitOfWork.ProductRepository.Add(productVM.Product);
                unitOfWork.Save();
                TempData["success"] = "Product Created successfully";
                return RedirectToAction("Index");

            }

            return View();
        }
        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();

            }
           
            IEnumerable<SelectListItem> categoryList = unitOfWork.CategoryRepository.GetAll()
                   ?.Select(i => new SelectListItem(i.Name, i.Id.ToString()))
                   ?? Enumerable.Empty<SelectListItem>(); // Fallback to empty list if null            

            ProductVM productVM = new ProductVM()
            {
                Product = unitOfWork.ProductRepository.Get(c => c.Id == id),
                CategoryList = categoryList
            };
            if (productVM.Product == null||productVM.CategoryList==null)
            {
                return NotFound();
            }
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file!=null)
                {
                  
                   DeleteOldImage(productVM.Product.ImageUrl);
                    productVM.Product.ImageUrl= ProcessUploadedImage(file);
                }
                unitOfWork.ProductRepository.Update(productVM.Product);
                unitOfWork.Save();
                TempData["success"] = "Product Updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpDelete]
        public IActionResult Delete(int? id) { 
            Product? product = unitOfWork.ProductRepository.Get(c=>c.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            DeleteOldImage(product.ImageUrl);
            unitOfWork.ProductRepository.Remove(product);
            unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });

        }
       
        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }
            try
            {
                // Combine with WebRootPath, removing any leading slash from imageUrl
                string imageIOPath = Path.Combine(webHostEnvironment.WebRootPath,
                    imageUrl.TrimStart('/', '\\'));

                // Check if file exists and delete it
                if (System.IO.File.Exists(imageIOPath))
                {
                    System.IO.File.Delete(imageIOPath);
                }
            }
            catch (Exception ex)
            {
                // Log the error (in a real application)
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }
        private string ProcessUploadedImage(IFormFile file)
        {
            string wwwRootPath = webHostEnvironment.WebRootPath;

            string imageName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
            string folderPath = Path.Combine(wwwRootPath, "images", "product");
            // Ensure the directory exists
            Directory.CreateDirectory(folderPath);
            string newImagePath = Path.Combine(folderPath, imageName);
            using (var fileStream = new FileStream(newImagePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            // Use forward slashes and ensure leading slash
            return Path.Combine("/", "images", "product", imageName).Replace("\\", "/");
        }



        #region API Calls
        [HttpGet]
        public IActionResult GetAll() {
            var products = unitOfWork.ProductRepository.GetAll("Category")
                .Select(p => new { p.Id,p.Title, p.ISBN, p.ListPrice, p.Author, CategoryName = p.Category.Name }).ToList();


            return Json(new {data=products});
        }
        #endregion
    }
}
