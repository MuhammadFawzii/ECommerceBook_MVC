using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.DataAcess.Data;
using ECommerceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext db;

        public CompanyRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void Update(Company entity)
        {
            dbSet.Update(entity);
        }
    }
}
