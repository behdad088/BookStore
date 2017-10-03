using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.UI.WebControls.Expressions;
using book.Controllers;
using book.Models;
using book.Models.Interfaces;
using book.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace bookstore.Tests.Controllers
{

    [TestFixture]
    public class UT_BookControllerTests
    {
        private Mock<IBookService> _bookServiceMock;

        [SetUp]
        public void SetupEachTest()
        {
            _bookServiceMock = new Mock<IBookService>(); ;
        }

        [Test]
        public void AddBook_InvalidModeStade_BadRequest()
        {
            //Arrange
            var bookstore = new Bookstore();
            var controller = new BookController(_bookServiceMock.Object);
            controller.ModelState.AddModelError("ModelError", "ModelError");

            //Act
            var action = controller.AddBook(bookstore);

            //Assert
            Assert.IsTrue(action is InvalidModelStateResult);
        }

        [Test]
        public void EditBook_InvalidModeStade_BadRequest()
        {
            //Arrange
            var bookstore = new Bookstore();
            var controller = new BookController(_bookServiceMock.Object);
            controller.ModelState.AddModelError("ModelError", "ModelError");

            //Act
            var action = controller.EditBook(bookstore);

            //Assert
            Assert.IsTrue(action is InvalidModelStateResult);
        }

        [Test]
        public void DeleteBook_InvalidModeStade_BadRequest()
        {
            //Arrange
            var bookId = 0;
            var controller = new BookController(_bookServiceMock.Object);

            //Act
            var action = controller.DeleteBook(bookId);

            //Assert
            Assert.IsTrue(action is BadRequestErrorMessageResult);
        }

        [Test]
        public void OrderBook_InvalidModeStade_BadRequest()
        {
            //Arrange
            var cart = Enumerable.Empty<Cart>();
            var controller = new BookController(_bookServiceMock.Object);
            controller.ModelState.AddModelError("ModelError", "ModelError");

            //Act
            var action = controller.OrderBook(cart);

            //Assert
            Assert.IsTrue(action is InvalidModelStateResult);
        }

        [Test]
        public void Get_GivenNoAuthorFragment_EmptyList()
        {
            //Arrange
            var query = string.Empty;
            var searchType = BookSearch.Author;
            var controller = new BookController(_bookServiceMock.Object);

            //Act
            var action = controller.Get(query, searchType);
            var okResult = action as OkNegotiatedContentResult<Task<IEnumerable<IBook>>>;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(0, okResult.Content.Result.Count());
        }

        [Test]
        public void Get_GivenNoTitleFragment_EmptyList()
        {
            //Arrange
            var query = string.Empty;
            var searchType = BookSearch.Title;
            var controller = new BookController(_bookServiceMock.Object);

            //Act
            var action = controller.Get(query, searchType);
            var okResult = action as OkNegotiatedContentResult<Task<IEnumerable<IBook>>>;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(0, okResult.Content.Result.Count());
        }

        [Test]
        public void Get_GivenTitleFragment_CorrectData()
        {
            //Arrange
            var data = new List<IBook> {
                new Bookstore
                {
                    Id = 1,
                    Author = "test Author",
                    Title = "test Title",
                    inStock = 6,
                    Price = 1000
                },
                 new Bookstore
                {
                    Id = 2,
                    Author = "Author2",
                    Title = "Title2",
                    inStock = 4,
                    Price = 5000
                }};

            _bookServiceMock.Setup(x => x.GetBooksByTitleAsync(It.IsAny<string>())).Returns(Task.FromResult<IEnumerable<IBook>>(data.Where( x=> x.Title.Contains("test"))));
            var controller = new BookController(_bookServiceMock.Object);

            //Act
            var action = controller.Get("test", BookSearch.Title);
            var okResult = action as OkNegotiatedContentResult<Task<IEnumerable<IBook>>>;

            //Assert
            Assert.IsNotNull(action);
            Assert.IsNotNull(okResult);

            var content = okResult.Content.Result.ToList();
            Assert.AreEqual(1, content.Count);
            Assert.AreEqual(1, content[0].Id);
            Assert.AreEqual("test Title", content[0].Title);
        }
        [Test]
        public void Get_GivenAuthorFragment_CorrectData()
        {
            //Arrange
            var data = new List<IBook> {
                new Bookstore
                {
                    Id = 1,
                    Author = "test Author",
                    Title = "test Title",
                    inStock = 6,
                    Price = 1000
                },
                 new Bookstore
                {
                    Id = 2,
                    Author = "Author2",
                    Title = "Title2",
                    inStock = 4,
                    Price = 5000
                }};

            _bookServiceMock.Setup(x => x.GetBooksByAuthorAsync(It.IsAny<string>())).Returns(Task.FromResult<IEnumerable<IBook>>(data.Where(x => x.Author.Contains("test"))));
            var controller = new BookController(_bookServiceMock.Object);

            //Act
            var action = controller.Get("test", BookSearch.Author);
            var okResult = action as OkNegotiatedContentResult<Task<IEnumerable<IBook>>>;

            //Assert
            Assert.IsNotNull(action);
            Assert.IsNotNull(okResult);

            var content = okResult.Content.Result.ToList();
            Assert.AreEqual(1, content.Count);
            Assert.AreEqual(1, content[0].Id);
            Assert.AreEqual("test Author", content[0].Author);
        }

        [Test]
        public void Get_GivenOutOfRangeBookSearchType_CorrectData()
        {
            //Arrange
            var bookSearchType = 7;
            var data = new List<IBook> {
                new Bookstore
                {
                    Id = 1,
                    Author = "test Author",
                    Title = "test Title",
                    inStock = 6,
                    Price = 1000
                },
                 new Bookstore
                {
                    Id = 2,
                    Author = "Author2",
                    Title = "Title2",
                    inStock = 4,
                    Price = 5000
                }};

            _bookServiceMock.Setup(x => x.GetBooksAsync()).Returns(Task.FromResult<IEnumerable<IBook>>(data));
            var controller = new BookController(_bookServiceMock.Object);

            //Act
            var action = controller.Get("test", (BookSearch)bookSearchType);
            var okResult = action as OkNegotiatedContentResult<Task<IEnumerable<IBook>>>;

            //Assert
            Assert.IsNotNull(action);
            Assert.IsNotNull(okResult);

            var content = okResult.Content.Result.ToList();
            Assert.AreEqual(2, content.Count);
            Assert.AreEqual(1, content[0].Id);
            Assert.AreEqual("test Title", content[0].Title);
        }

        [Test]
        public void Get_AllOrderInformation_CorrectData()
        {
            //Arrange
            var data = new List<OrderItemInfo>
            {
                new OrderItemInfo
                {
                    BookId = 1,
                    OrderId = Guid.NewGuid(),
                    Author = "test Author1",
                    Title = "test Title1",
                    Number = 1,
                    TotalPrice = 240,
                },
                new OrderItemInfo
                {
                    BookId = 2,
                    OrderId = Guid.NewGuid(),
                    Author = "test Author2",
                    Title = "test Title2",
                    Number = 3,
                    TotalPrice = 540,
                }
            };

            _bookServiceMock.Setup(x => x.GetOrderAsync()).Returns(Task.FromResult<IEnumerable<IOrderItemInfo>>(data));
            var controller = new BookController(_bookServiceMock.Object);

            //Act
            var action = controller.GetOrder();
            var okResult = action as OkNegotiatedContentResult<Task<IEnumerable<IOrderItemInfo>>>;

            //Assert
            Assert.IsNotNull(action);
            Assert.IsNotNull(okResult);

            var content = okResult.Content.Result.ToList();
            Assert.AreEqual(2, content.Count);
            Assert.AreEqual(1, content[0].BookId);
            Assert.AreEqual("test Author1", content[0].Author);
            Assert.AreEqual("test Author2", content[1].Author);
        }
    }
}
