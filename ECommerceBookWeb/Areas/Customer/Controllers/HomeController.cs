using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
