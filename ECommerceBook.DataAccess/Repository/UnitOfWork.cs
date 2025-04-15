using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.DataAcess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;
        public ICategoryRepository CategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext _db) 
        {
            db = _db;
            CategoryRepository = new CategoryRepository(_db);
            ProductRepository = new ProductRepository(_db);
            CompanyRepository = new CompanyRepository(_db);
            ShoppingCartRepository = new ShoppingCartRepository(_db);
            ApplicationUserRepository = new ApplicationUserRepository(_db);
        }


        public void Save()
        {
           db.SaveChanges();
        }
    }
}
