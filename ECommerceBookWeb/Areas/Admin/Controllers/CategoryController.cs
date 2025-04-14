using ECommerceBook.DataAcess.Data;
using ECommerceBook.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerceBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using ECommerceBook.Utility;
namespace ECommerceBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CategoryController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoriesList = unitOfWork.CategoryRepository.GetAll();
            return View(categoriesList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            //Custom Validation Message
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "the Display Order can't exactly match the Name.");
                //ModelState.AddModelError("Name", "the Display Order can't exactly match the Name.");

            }
            if (ModelState.IsValid)
            {
                unitOfWork.CategoryRepository.Add(category);
                unitOfWork.Save();
                TempData["success"] = "Category Created successfully";
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
            Category? category = unitOfWork.CategoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            //Custom Validation Message
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "the Display Order can't exactly match the Name.");
                //ModelState.AddModelError("Name", "the Display Order can't exactly match the Name.");

            }
            if (ModelState.IsValid)
            {
                unitOfWork.CategoryRepository.Update(category);
                unitOfWork.Save();
                TempData["success"] = "Category Updated successfully";

                return RedirectToAction("Index");
            }
            return View();



        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            Category? category = unitOfWork.CategoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? category = unitOfWork.CategoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            unitOfWork.CategoryRepository.Remove(category);
            unitOfWork.Save();
            TempData["success"] = "Category Deleted successfully";

            return RedirectToAction("Index");



        }
    }
}
