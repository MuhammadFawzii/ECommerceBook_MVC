using ECommerceBookRazor_Temp.Data;
using ECommerceBookRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceBookRazor_Temp.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category category { get; set; }
        public DeleteModel(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public void OnGet(int? id)
        {
            if (id != null || id != 0)
            {
                category = _db.Categories.Find(id);
            }

        }
        public IActionResult OnPost()
        {
            Category? obj = _db.Categories.Find(category.Id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToPage("Index");
           

        }
    }

}
