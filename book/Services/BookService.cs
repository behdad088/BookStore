using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using book.Infrastructure;
using book.Infrastructure.Interfaces;
using book.Models;
using book.Models.Interfaces;
using book.Services.Interfaces;

namespace book.Services
{
    public class BookService : IBookService
    {

        private readonly IUnitOfWork _unitOfWork;
        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IBook>> GetBooksAsync()
        {
            var bookRepository = _unitOfWork.BookstoreRepository;
            var books = await bookRepository.Get();
            return books;
        }

        public async Task<IEnumerable<IBook>> GetBooksByAuthorAsync(string author)
        {
            var bookRepository = _unitOfWork.BookstoreRepository;
            var books = await bookRepository.Get(bookstore => bookstore.Author.Contains(author));
            return books;
        }

        public async Task<IEnumerable<IBook>> GetBooksByTitleAsync(string title)
        {
            var bookRepository = _unitOfWork.BookstoreRepository;
            var books = await bookRepository.Get(bookstore => bookstore.Title.Contains(title));
            return books;
        }

        public async Task<bool> AddBookAsync(Bookstore book)
        {
            var bookRepository = _unitOfWork.BookstoreRepository;
            if (book == null) return false;
            
            await bookRepository.Insert(book);
            _unitOfWork.Save();
            return true;
            
        }

        public async Task<bool> EditBookAsync(Bookstore book)
        {
            var bookRepository = _unitOfWork.BookstoreRepository;
            if (book.Id <= 0)
            {
                return false;
            }
            try
            {
                await bookRepository.Update(book);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                throw;
            }
            
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var bookRepository = _unitOfWork.BookstoreRepository;
            try
            {
                if (id <= 0)
                {
                    return false;
                }
                await bookRepository.Delete(id);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                throw;
            }
        }

        public async Task<bool> OrderBookAsync(IEnumerable<Cart> cart)
        {
            foreach (var item in cart)
            {
                var bookId = item.BookId;

                var orderRepository = _unitOfWork.OrderRepository;
                var bookRepository = _unitOfWork.BookstoreRepository;

                var book = bookRepository.GetByID(bookId).Result;

                if (book == null) return false;

                if (book.inStock - item.Number < 0)
                {
                    return false;
                }

                var order = new Order
                {
                    OrderId = Guid.NewGuid(),
                    BookId = bookId,
                    Number = item.Number,
                    TotalPrice = book.Price * item.Number
                };

                try
                {
                    await orderRepository.Insert(order);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    throw;
                }

                book.inStock -= item.Number;
                await bookRepository.Update(book);
                _unitOfWork.Save();
                return true;
                
                
            }

            return false;
        }

        public async Task<IEnumerable<IOrderItemInfo>> GetOrderAsync()
        {
            var orderRepository = _unitOfWork.OrderRepository;
            var bookRepository = _unitOfWork.BookstoreRepository;
        
            var result =  orderRepository.GetAll().Join(bookRepository.GetAll(),
                                                        order => order.BookId,
                                                        book => book.Id,
                                                        (ord, bo) => new OrderItemInfo()
                                                        {
                                                            OrderId = ord.OrderId,
                                                            BookId = bo.Id,
                                                            Author =  bo.Author,
                                                            Title = bo.Title,
                                                            Number = ord.Number,
                                                            TotalPrice = ord.TotalPrice
                                                        });

            return result;
        }
    }
}