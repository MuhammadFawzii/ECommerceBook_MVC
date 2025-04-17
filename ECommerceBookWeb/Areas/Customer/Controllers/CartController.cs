using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.Models;
using ECommerceBook.Models.ViewModels;
using ECommerceBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;
using System.Security.Claims;

namespace ECommerceBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        [BindProperty]
        public CarVM CarVM { get; set; }
        public CartController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            
            CarVM = new CarVM()
            {
                ShoppingListCart = unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId==GetUserId(), includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };
            AssignTotalOrder(CarVM);
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
        [HttpGet]
        public IActionResult Summary()
        {
         
            CarVM = new CarVM()
            {
                ShoppingListCart = unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId==GetUserId(), includeProperties: "Product"),
                OrderHeader = new OrderHeader()


            };
            CarVM.OrderHeader.ApplicationUser = unitOfWork.ApplicationUserRepository.Get(u => u.Id==GetUserId());
            CarVM.OrderHeader.Name = CarVM.OrderHeader.ApplicationUser.Name;
            CarVM.OrderHeader.PhoneNumber = CarVM.OrderHeader.ApplicationUser.PhoneNumber;
            CarVM.OrderHeader.StreetAddress = CarVM.OrderHeader.ApplicationUser.StreetAddress;
            CarVM.OrderHeader.City = CarVM.OrderHeader.ApplicationUser.City;
            CarVM.OrderHeader.State = CarVM.OrderHeader.ApplicationUser.State;
            CarVM.OrderHeader.PostalCode = CarVM.OrderHeader.ApplicationUser.PostalCode;
            AssignTotalOrder(CarVM);
            return View(CarVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
           
            CarVM.ShoppingListCart = unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId==GetUserId(), includeProperties: "Product");
            CarVM.OrderHeader.OrderDate=DateTime.Now;
            CarVM.OrderHeader.ApplicationUserId=GetUserId();
            CarVM.OrderHeader.ApplicationUser = unitOfWork.ApplicationUserRepository.Get(u => u.Id==GetUserId());
            AssignTotalOrder(CarVM);
           
            if(CarVM.OrderHeader.ApplicationUser.CompanyId.GetValueOrDefault()==0)
            {
                //This is a customer 
                CarVM.OrderHeader.PaymentStatus=SD.PaymentStatusPending;
                CarVM.OrderHeader.OrderStatus=SD.StatusPending;

               
            }
            else
            {
                //This is a company
                CarVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                CarVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            unitOfWork.OrderHeaderRepository.Add(CarVM.OrderHeader);
            unitOfWork.Save();
            foreach (var item in CarVM.ShoppingListCart)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    ProductId=item.ProductId,
                    OrderHeaderId=CarVM.OrderHeader.Id,
                    Count=item.Count,
                    Price=item.Price
                };
                unitOfWork.OrderDetailRepository.Add(orderDetail);
                unitOfWork.Save();
            }
            if (CarVM.OrderHeader.ApplicationUser.CompanyId.GetValueOrDefault()==0)
            {
                var domain = "http://localhost:5075/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain+$"customer/cart/orderConfirmation?headerId={CarVM.OrderHeader.Id}",
                    CancelUrl = domain+$"customer/cart/index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };
                foreach (var item in CarVM.ShoppingListCart)
                {
                    var sessionLineItem = new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData=new Stripe.Checkout.SessionLineItemPriceDataOptions
                        {
                            UnitAmount=(long)(item.Price*100),
                            Currency="usd",
                            ProductData=new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name=item.Product?.Title,
                            },
                        },
                        Quantity=item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new Stripe.Checkout.SessionService();
                var session = service.Create(options);
                unitOfWork.OrderHeaderRepository.UpdateStripePaymentId(CarVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
           return RedirectToAction(nameof(OrderConfirmation), new {headerId=CarVM.OrderHeader.Id});
        }
        public IActionResult OrderConfirmation(int headerId)
        {
            var orderHeader = unitOfWork.OrderHeaderRepository.Get(u => u.Id==headerId, includeProperties: "ApplicationUser");
            if (orderHeader.PaymentStatus!=SD.PaymentStatusDelayedPayment)
            {
                var service = new Stripe.Checkout.SessionService();
                var session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower()=="paid")
                {
                    unitOfWork.OrderHeaderRepository.UpdateStripePaymentId(headerId, session.Id, session.PaymentIntentId);
                    unitOfWork.OrderHeaderRepository.UpdateStatus(headerId, SD.StatusApproved, SD.PaymentStatusApproved);
                    unitOfWork.Save();
                }
            }
            List<ShoppingCart> shoppingCarts = unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId==orderHeader.ApplicationUserId).ToList();
            unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);
            unitOfWork.Save();
            return View(headerId);
        }
        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            return UserId;
        }
        private void AssignTotalOrder(CarVM CarVM)
        {
            foreach (var Cart in CarVM.ShoppingListCart)
            {
                Cart.Price = GetPriceBasedOnCount(Cart);
                CarVM.OrderHeader.OrderTotal+=(double)(Cart.Price * Cart.Count);
            }
        }
        private double GetPriceBasedOnCount(ShoppingCart Cart)
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
