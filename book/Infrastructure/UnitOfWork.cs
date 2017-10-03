using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using book.Infrastructure.Interfaces;
using book.Models;
using book.Models.Interfaces;
using book.Repository;

namespace book.Infrastructure
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private RepositoryContext context = new RepositoryContext();
        private IGenericRepository<Bookstore> bookstoreRepository;
        private IGenericRepository<Order> orderRepository;

        public IGenericRepository<Bookstore> BookstoreRepository
        {
            get
            {
                if (this.bookstoreRepository == null)
                {
                    this.bookstoreRepository = new GenericRepository<Bookstore>(context);
                }
                return bookstoreRepository;
            }
        }

        public IGenericRepository<Order> OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository= new GenericRepository<Order>(context);
                }
                return orderRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}