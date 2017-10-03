using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using book.Models.Interfaces;

namespace book.Models
{
    public class OrderItemInfo : IOrderItemInfo
    {
        public Guid OrderId { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Number { get; set; }
        public decimal TotalPrice { get; set; }
    }
}