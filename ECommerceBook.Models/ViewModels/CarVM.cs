using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBook.Models.ViewModels
{
    public class CarVM
    {
        public IEnumerable<ShoppingCart> ShoppingListCart { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}
