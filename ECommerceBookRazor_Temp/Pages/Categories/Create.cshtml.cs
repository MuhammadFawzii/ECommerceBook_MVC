using ECommerceBookRazor_Temp.Data;
using ECommerceBookRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceBookRazor_Temp.Pages.Categories
{
    //[BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category category { get; set; }
        public CreateModel(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public void OnGet()
        {
            
        }
        public IActionResult OnPost() {
            if (ModelState.IsValid) {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category Created successfully";
                return RedirectToPage("Index");
            }
            return Page();

        }
    }
}
