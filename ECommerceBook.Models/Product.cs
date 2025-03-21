using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBook.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Title is required")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Product ISBN is required")]
        //"978-1-234567-89-4"
        [RegularExpression(@"^\d{3}-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$",
            ErrorMessage = "Invalid ISBN format. Example: 978-1-234567-89-0")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "Product Author is required")]
        public string? Author { get; set; }

        [Required(ErrorMessage = "Product List Price is required")]
        [Range(1, 1000, ErrorMessage = "List Price must be between 1 and 1000.")]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }

        [Required(ErrorMessage = "Product Price for 1-50 is required")]
        [Range(1, 1000, ErrorMessage = "Price for 1-50 must be between 1 and 1000.")]
        [Display(Name = "Price for 1-50")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Product Price for 50+ is required")]
        [Range(1, 1000, ErrorMessage = "Price for 50+ must be between 1 and 1000.")]
        [Display(Name = "Price for 50+")]
        public double Price50 { get; set; }

        [Required(ErrorMessage = "Product Price for 100+ is required")]
        [Range(1, 1000, ErrorMessage = "Price for 100+ must be between 1 and 1000.")]
        [Display(Name = "Price for 100+")]
        public double Price100 { get; set; }
    }
}
