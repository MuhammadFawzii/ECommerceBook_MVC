using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.Models;
using ECommerceBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartCarController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CarVM CarVM { get; set; }
        public CartCarController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            CarVM = new CarVM()
            {
                ShoppingListCart = unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId==UserId, includeProperties: "Product")

            };
            foreach(var Cart in CarVM.ShoppingListCart)
            {
                Cart.Price = GetPriceBasedOnCount(Cart);
                CarVM.OrderTotal+=(double)(Cart.Price * Cart.Count);
            }
            return View(CarVM);
        }
        public IActionResult Plus(int cartId)
        {
            var shoppingCartDB = unitOfWork.ShoppingCartRepository.Get(u => u.Id==cartId);
            shoppingCartDB.Count += 1;
            unitOfWork.ShoppingCartRepository.Update(shoppingCartDB);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var shoppingCartDB = unitOfWork.ShoppingCartRepository.Get(u => u.Id==cartId);
            if (shoppingCartDB.Count<=1)
            {
                unitOfWork.ShoppingCartRepository.Remove(shoppingCartDB);
            }
            else
            {
                shoppingCartDB.Count -= 1;
                unitOfWork.ShoppingCartRepository.Update(shoppingCartDB);
            }
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var shoppingCartDB = unitOfWork.ShoppingCartRepository.Get(u => u.Id==cartId);
            unitOfWork.ShoppingCartRepository.Remove(shoppingCartDB);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            return View();
        }
        private double? GetPriceBasedOnCount(ShoppingCart Cart)
        {
            if (Cart.Count<=50)
            {
                return Cart.Product.Price;
            }
            else if(Cart.Count>50&& Cart.Count<=100)
            {
                return Cart.Product.Price50;
            }
            else
            {
                return Cart.Product.Price100;
            }
        }


    }
}
