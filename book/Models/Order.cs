using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using book.Models.Interfaces;

namespace book.Models
{
    public class Order : IOrder
    {
        public Guid OrderId { get; set; }
        public int BookId { get; set; }
        public int Number { get; set; }
        public decimal TotalPrice { get; set; }

    }
}