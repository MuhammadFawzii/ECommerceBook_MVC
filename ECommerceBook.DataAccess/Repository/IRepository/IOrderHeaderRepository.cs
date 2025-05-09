﻿using ECommerceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository:IRepository<OrderHeader>
    {
        public void Update(OrderHeader entity);
        public void UpdateStatus(int orderHeaderId, string orderStatus, string? paymentStatus = null);
        public void UpdateStripePaymentId(int orderHeaderId, string sessionId, string paymentIntentId);
    }

}
