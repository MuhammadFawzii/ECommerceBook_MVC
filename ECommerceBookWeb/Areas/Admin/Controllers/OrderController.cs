using ECommerceBook.DataAccess.Repository;
using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.Models;
using ECommerceBook.Models.ViewModels;
using ECommerceBook.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace ECommerceBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public OrderController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderHeaderId) {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader=unitOfWork.OrderHeaderRepository.Get(u => u.Id==orderHeaderId, includeProperties: "ApplicationUser"),
                OrderDetails=unitOfWork.OrderDetailRepository.GetAll(u => u.OrderHeaderId==orderHeaderId, includeProperties: "Product")
            };
            return View(orderVM);
        }
        #region API Calls
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaderList = unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "ApplicationUser")
                
                .ToList();
            switch (status)
            {
                case "pending":
                    orderHeaderList =orderHeaderList.Where(u => u.PaymentStatus==SD.PaymentStatusPending);
                    break;
                case "inprocess":
                    orderHeaderList = orderHeaderList.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaderList = orderHeaderList.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaderList = orderHeaderList.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }
            return Json(new { data = orderHeaderList });
        }
        #endregion
    }
}
