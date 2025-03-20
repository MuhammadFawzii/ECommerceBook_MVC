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


        public UnitOfWork(ApplicationDbContext _db) 
        {
            db = _db;
            CategoryRepository = new CategoryRepository(_db);
        }


        public void Save()
        {
           db.SaveChanges();
        }
    }
}
