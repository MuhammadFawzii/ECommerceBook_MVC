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
    public class ShoppingCartRepository  :Repository<ShoppingCart>,  IShoppingCartRepository
    {
        private ApplicationDbContext db;
        public ShoppingCartRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void Update(ShoppingCart entity)
        {
            dbSet.Update(entity);//check
        }
      


    }
}
