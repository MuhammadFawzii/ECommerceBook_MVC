using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ECommerceBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork _unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = _unitOfWork;

        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = unitOfWork.ProductRepository.GetAll(includeProperties: "Category");

            return View(products);
        }
        [HttpGet]
        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = unitOfWork.ProductRepository.Get(p => p.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };
            return View(shoppingCart);

        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var UserId=claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                shoppingCart.ApplicationUserId = UserId;
                ShoppingCart cartFromDb=unitOfWork.ShoppingCartRepository.Get(u=>u.ApplicationUserId==shoppingCart.ApplicationUserId && u.ProductId==shoppingCart.ProductId);
                if (cartFromDb==null)
                {
                    unitOfWork.ShoppingCartRepository.Add(shoppingCart);
                }
                else
                {
                    cartFromDb.Count += shoppingCart.Count;
                    unitOfWork.ShoppingCartRepository.Update(cartFromDb);
                }
            }
            unitOfWork.Save();
            TempData["success"] = "Shopping cart created successfully";
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
