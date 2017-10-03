using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using book.Models;
using book.Services;
using book.Services.Interfaces;
using System.Web.Http.Cors;
using book.Models.Interfaces;

namespace book.Controllers
{
    public enum BookSearch
    {
        All = 0,
        Title = 1,
        Author = 2
    }

    [RoutePrefix("api/Bookstore")]
    public class BookController : ApiController
    {
        private readonly IBookService _bookService;


        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }


        [Route("addbook")]
        [HttpPost]
        public IHttpActionResult AddBook(Bookstore book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_bookService.AddBookAsync(book));
        }

        [Route("editbook")]
        [HttpPut]
        public IHttpActionResult EditBook(Bookstore book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_bookService.EditBookAsync(book));
        }

        [Route("deletebook")]
        [HttpDelete]
        public IHttpActionResult DeleteBook(int id)
        {
            if (id <= 0)
            {
                return BadRequest("BookId value is not Valid.");
            }
            return Ok(_bookService.DeleteBookAsync(id));
        }

        [Route("orderbook")]
        [HttpPost]
        public IHttpActionResult OrderBook(IEnumerable<Cart> cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_bookService.OrderBookAsync(cart));
        }

        [Route("search")]
        [HttpGet]
        public IHttpActionResult Get(string query, BookSearch searchType)
        {
            if (string.IsNullOrWhiteSpace(query) && string.IsNullOrWhiteSpace(searchType.ToString()))
            {
                return Ok(Enumerable.Empty<IBook>());
            }

            switch (searchType)
            {
                case BookSearch.All:
                    return Ok(_bookService.GetBooksAsync());
                case BookSearch.Title:
                    return Ok(_bookService.GetBooksByTitleAsync(query));
                case BookSearch.Author:
                    return Ok(_bookService.GetBooksByAuthorAsync(query));
                default:
                    return Ok(_bookService.GetBooksAsync());
            }

        }

        [Route("order")]
        [HttpGet]
        public IHttpActionResult GetOrder()
        {
            return Ok(_bookService.GetOrderAsync());
        }

    }
}