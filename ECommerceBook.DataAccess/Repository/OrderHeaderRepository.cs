using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.DataAcess.Data;
using ECommerceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext db;
        public OrderHeaderRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void Update(OrderHeader entity)
        {
            dbSet.Update(entity);//check
        }

        public void UpdateStatus(int orderHeaderId, string orderStatus, string? paymentStatus = null)
        {
            OrderHeader orderHeaderFromDb =db.OrderHeaders.FirstOrDefault(u=>u.Id==orderHeaderId) ;
            if(orderHeaderFromDb !=null)
            {
                orderHeaderFromDb.OrderStatus = orderStatus;
                if(!string.IsNullOrEmpty(paymentStatus))
                {
                    orderHeaderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int orderHeaderId, string sessionId, string paymentIntentId)
        {
            OrderHeader orderHeaderFromDb = db.OrderHeaders.FirstOrDefault(u => u.Id==orderHeaderId);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderHeaderFromDb.SessionId = sessionId;
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    orderHeaderFromDb.PaymentIntentId = paymentIntentId;
                    orderHeaderFromDb.PaymentDate = DateTime.Now;
                }
            }
        }
    }
}
