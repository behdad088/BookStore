using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace book.Models.Interfaces
{
    public interface IBook
    {
        int Id { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        decimal Price { get; set; }
        int inStock { get; set; }
    }
}
