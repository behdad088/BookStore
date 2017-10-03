using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using book.Models;
using book.Models.Interfaces;

namespace book.Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext() : base("name=BookStoreDatabase")
        {
        }

        public System.Data.Entity.DbSet<Bookstore> Bookstore { get; set; }
        public System.Data.Entity.DbSet<Order> Orders { get; set; }
    }
}