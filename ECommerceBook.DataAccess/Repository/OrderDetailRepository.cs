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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext db;
        public OrderDetailRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void Update(OrderDetail entity)
        {
            dbSet.Update(entity);//check
        }
      


    }
}
