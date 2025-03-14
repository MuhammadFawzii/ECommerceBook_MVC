using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECommerceBookRazor_Temp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(30, ErrorMessage = "Category Name cannot exceed 30 characters.")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Required(ErrorMessage = "Display Order field is required")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100.")]
        public int DisplayOrder { get; set; }
    }
}
