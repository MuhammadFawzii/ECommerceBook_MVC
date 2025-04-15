using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        public void Save();

    }
}
