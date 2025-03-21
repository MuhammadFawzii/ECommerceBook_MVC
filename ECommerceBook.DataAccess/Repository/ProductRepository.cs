using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.DataAcess.Data;
using ECommerceBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext db;
        public ProductRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }

        public void Update(Product entity)
        {
            dbSet.Update(entity);

        }
    }
}
