using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using book.Models;
using book.Models.Interfaces;

namespace book.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Bookstore> BookstoreRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        void Save();
        void Dispose(bool disposing);
    }
}
