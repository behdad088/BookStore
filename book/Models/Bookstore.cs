using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using book.Models.Interfaces;

namespace book.Models
{
    public class Bookstore : IBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int inStock { get; set; }
    }
}