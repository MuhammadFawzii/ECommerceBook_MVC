using ECommerceBook.DataAccess.Repository;
using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerceBook.Models.ViewModels;
namespace ECommerceBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork _unitOfWork) {
            unitOfWork = _unitOfWork;

        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = unitOfWork.ProductRepository.GetAll();
            return View(products);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = unitOfWork.CategoryRepository.GetAll()
                    ?.Select(i => new SelectListItem(i.Name, i.Id.ToString()))
                    ?? Enumerable.Empty<SelectListItem>(); // Fallback to empty list if null            
            //ViewBag.CategoryList = categoryList;
            //ViewData["CategoryList"] = categoryList;
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = categoryList
            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
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
            Product? product = unitOfWork.ProductRepository.Get(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ProductRepository.Update(product);
                unitOfWork.Save();
                TempData["success"] = "Product Updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id) { 
            Product? product = unitOfWork.ProductRepository.Get(c=>c.Id == id);
            if (product == null)
            {
                TempData["error"] = "Product not found";
                return RedirectToAction("Index");
            }
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? product = unitOfWork.ProductRepository.Get(c => c.Id == id);
            if (product == null)
            {
                TempData["error"] = "Product not found";
                return RedirectToAction("Index");
            }
            unitOfWork.ProductRepository.Remove(product);
            unitOfWork.Save();
            TempData["success"] = "Product Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
