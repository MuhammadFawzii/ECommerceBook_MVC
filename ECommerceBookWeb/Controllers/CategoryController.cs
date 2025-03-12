using ECommerceBookWeb.Data;
using ECommerceBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> categoriesList=_db.Categories.ToList();
            return View(categoriesList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category) {
            //Custom Validation Message
            if (category.Name==category.DisplayOrder.ToString()) {
                ModelState.AddModelError("", "the Display Order can't exactly match the Name.");
                //ModelState.AddModelError("Name", "the Display Order can't exactly match the Name.");

            }
            if (ModelState.IsValid) {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"]= "Category Created successfully";
                return RedirectToAction("Index");
            }
            return View();
            

        
        }
        public IActionResult Edit(int?id)
        {
            if (id == null||id==0) { 
                return NotFound();
           
            }
            Category? category = _db.Categories.Find(id);
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
                _db.Categories.Update(category);
                _db.SaveChanges();
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
            Category? category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int?id)
        {
            Category? category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category Deleted successfully";

            return RedirectToAction("Index");
           


        }
    }
}
