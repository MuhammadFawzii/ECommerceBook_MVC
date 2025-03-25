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

        public void Update(Product prod)
        {
            //dbSet.Update(entity);
            Product? product = dbSet.FirstOrDefault(p => p.Id == prod.Id);
            if (product != null)
            {
                if (prod.ImageUrl != null)
                {
                    product.ImageUrl = prod.ImageUrl;
                }
                product.Title = prod.Title;
                product.Description = prod.Description;
                product.ISBN = prod.ISBN;
                product.Author = prod.Author;
                product.ListPrice = prod.ListPrice;
                product.Price = prod.Price;
                product.Price50 = prod.Price50;
                product.Price100 = prod.Price100;
                product.CategoryId = prod.CategoryId;
            }
        }
    }
}
