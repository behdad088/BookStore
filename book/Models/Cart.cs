using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace book.Models
{
    public class Cart
    {
        public int BookId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }
    }
}