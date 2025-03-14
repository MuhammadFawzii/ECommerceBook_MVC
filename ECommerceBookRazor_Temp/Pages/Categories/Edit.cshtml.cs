using ECommerceBookRazor_Temp.Data;
using ECommerceBookRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceBookRazor_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category category { get; set; }
        public EditModel(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public void OnGet(int? id)
        {
            if(id != null || id != 0)
            {
                category=_db.Categories.Find(id);
            }

        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category Updated successfully";
                return RedirectToPage("Index");
            }
            return Page();

        }
    }
}
