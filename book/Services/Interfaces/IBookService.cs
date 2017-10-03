using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using book.Models;
using book.Models.Interfaces;

namespace book.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<IBook>> GetBooksAsync();
        Task<IEnumerable<IOrderItemInfo>> GetOrderAsync();
        Task<IEnumerable<IBook>> GetBooksByAuthorAsync(string author);
        Task<IEnumerable<IBook>> GetBooksByTitleAsync(string title);
        Task<bool> AddBookAsync(Bookstore book);
        Task<bool> EditBookAsync(Bookstore book);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> OrderBookAsync(IEnumerable<Cart> cart);
    }
}
