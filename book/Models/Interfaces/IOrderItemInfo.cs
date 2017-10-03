using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace book.Models.Interfaces
{
    public interface IOrderItemInfo
    {
        Guid OrderId { get; set; }
        int BookId { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        int Number { get; set; }
        decimal TotalPrice { get; set; }
    }
}
