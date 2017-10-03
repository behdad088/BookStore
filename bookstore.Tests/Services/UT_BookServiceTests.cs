using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using book.Infrastructure.Interfaces;
using book.Models;
using book.Models.Interfaces;
using book.Repository;
using book.Services;
using Moq;
using NUnit.Framework;

namespace bookstore.Tests.Services
{
    [TestFixture]
    public class UT_BookServiceTests
    {
        private Mock<IUnitOfWork> _uniOfWorkMock;

        [SetUp]
        public void SetupEachTest()
        {
            _uniOfWorkMock = new Mock<IUnitOfWork>();

        }

        [Test]
        public void GetBook_GetBooksAsync_CorrectList()
        {
            //Arrange
             var data = new List<Bookstore> {
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
            
            Mock<IGenericRepository<Bookstore>> generic = new Mock<IGenericRepository<Bookstore>>();
            
            _uniOfWorkMock.Setup(x => x.BookstoreRepository.Get())
                .Returns(Task.FromResult<IEnumerable<Bookstore>>(data));

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.GetBooksAsync();

            //Assert
            var content = action.Result.ToList();
            Assert.AreEqual(2,content.Count());
            Assert.AreEqual("test Author", content[0].Author);

        }

        [Test]
        public void GetBook_GetBooksByAuthorAsync_CorrectList()
        {
            //Arrange
            var query = "test";
            var data = new List<Bookstore> {
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
                    Id = 1,
                    Author = "Author",
                    Title = "Title",
                    inStock = 6,
                    Price = 1000
                }
            };

            Mock<IGenericRepository<Bookstore>> generic = new Mock<IGenericRepository<Bookstore>>();

            _uniOfWorkMock.Setup(x => x.BookstoreRepository.Get(It.IsAny<Expression<Func<Bookstore, bool>>>()))
                .Returns(Task.FromResult<IEnumerable<Bookstore>>(data.Where(bo => bo.Author.Contains(query))));

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.GetBooksByAuthorAsync(query);

            //Assert
            var content = action.Result.ToList();
            Assert.AreEqual(1, content.Count());
            Assert.AreEqual("test Author", content[0].Author);

        }
        [Test]
        public void AddBook_Correctdata_SaveData_ReturnTrue()
        {
            //Arrange
            var data = 
                new Bookstore
                {
                    Id = 1,
                    Author = "test Author",
                    Title = "test Title",
                    inStock = 6,
                    Price = 1000
                };

            Mock<IGenericRepository<Bookstore>> generic = new Mock<IGenericRepository<Bookstore>>();

            _uniOfWorkMock.Setup(x => x.BookstoreRepository.Insert(It.IsAny<Bookstore>()))
                .Returns(Task.FromResult<bool>(true));

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.AddBookAsync(data);

            //Assert
            var content = action.Result;

            Assert.IsTrue(content);
            _uniOfWorkMock.Verify(u => u.Save(), Times.Exactly(1));
            _uniOfWorkMock.Verify(u => u.BookstoreRepository.Insert(It.IsAny<Bookstore>()), Times.Exactly(1));

        }

        [Test]
        public void AddBook_EmptyData_ReturnFalse()
        {
            //Arrange
            Bookstore data = null;

            Mock<IGenericRepository<Bookstore>> generic = new Mock<IGenericRepository<Bookstore>>();

            _uniOfWorkMock.Setup(x => x.BookstoreRepository.Insert(It.IsAny<Bookstore>()))
                .Returns(Task.FromResult<bool>(false));

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.AddBookAsync(data);

            //Assert
            var content = action.Result;

            Assert.IsFalse(content);
            _uniOfWorkMock.Verify(u => u.Save(), Times.Exactly(0));
            _uniOfWorkMock.Verify(u => u.BookstoreRepository.Insert(It.IsAny<Bookstore>()), Times.Exactly(0));

        }

        [Test]
        public void EditBook_CorrectValue_ReturnTrue()
        {
            //Arrange
            var data =
               new Bookstore
               {
                   Id = 1,
                   Author = "test Author",
                   Title = "test Title",
                   inStock = 6,
                   Price = 1000
               };

            Mock<IGenericRepository<Bookstore>> generic = new Mock<IGenericRepository<Bookstore>>();

            _uniOfWorkMock.Setup(x => x.BookstoreRepository.Update(It.IsAny<Bookstore>()))
                .Returns(Task.FromResult<bool>(true));

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.EditBookAsync(data);

            //Assert
            var content = action.Result;

            Assert.IsTrue(content);
            _uniOfWorkMock.Verify(u => u.Save(), Times.Exactly(1));
            _uniOfWorkMock.Verify(u => u.BookstoreRepository.Update(It.IsAny<Bookstore>()), Times.Exactly(1));

        }

        [Test]
        public void EditBook_NullBookId_ReturnFalse()
        {
            //Arrange
            var data =
               new Bookstore
               {
                   Author = "test Author",
                   Title = "test Title",
                   inStock = 6,
                   Price = 1000
               };

            Mock<IGenericRepository<Bookstore>> generic = new Mock<IGenericRepository<Bookstore>>();

            _uniOfWorkMock.Setup(x => x.BookstoreRepository.Update(It.IsAny<Bookstore>()))
                .Returns(Task.FromResult<bool>(false));

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.EditBookAsync(data);

            //Assert
            var content = action.Result;

            Assert.IsFalse(content);
            _uniOfWorkMock.Verify(u => u.Save(), Times.Exactly(0));
            _uniOfWorkMock.Verify(u => u.BookstoreRepository.Update(It.IsAny<Bookstore>()), Times.Exactly(0));

        }

        [Test]
        public void DeleteBook_NullBookId_ReturnFalse()
        {
            //Arrange
            var bookId = 0;

            Mock<IGenericRepository<Bookstore>> generic = new Mock<IGenericRepository<Bookstore>>();

            _uniOfWorkMock.Setup(x => x.BookstoreRepository.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult<bool>(false));

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.DeleteBookAsync(bookId);

            //Assert
            var content = action.Result;

            Assert.IsFalse(content);
            _uniOfWorkMock.Verify(u => u.Save(), Times.Exactly(0));
            _uniOfWorkMock.Verify(u => u.BookstoreRepository.Delete(It.IsAny<Bookstore>()), Times.Exactly(0));

        }

        [Test]
        public void DeleteBook_ValidId_ReturnTrue()
        {
            //Arrange
            var bookId = 2;

            Mock<IGenericRepository<Bookstore>> generic = new Mock<IGenericRepository<Bookstore>>();

            _uniOfWorkMock.Setup(x => x.BookstoreRepository.Delete(It.IsAny<Bookstore>()))
                .Returns(Task.FromResult<bool>(true));

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.DeleteBookAsync(bookId);

            //Assert
            var content = action.Result;

            Assert.IsTrue(content);
            _uniOfWorkMock.Verify(u => u.Save(), Times.Exactly(1));
            _uniOfWorkMock.Verify(u => u.BookstoreRepository.Delete(It.IsAny<int>()), Times.Exactly(1));

        }

        [Test]
        public void SubmitOrder_OutOfStockRange_ReturnFalse()
        {
            //Arrange
            var cart = new List<Cart>
            {
                new Cart
                {
                    BookId = 1,
                    Author = "test Author",
                    Title = "test Title",
                    Number = 7,
                    Price = 2000
                }
            };

            var order = new List<Order>
            {
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    BookId = 1,
                    Number = 2,
                    TotalPrice = 4000
                }
            };

            var book =
                new Bookstore
                {
                    Id = 1,
                    Author = "test Author",
                    Title = "test Title",
                    inStock = 6,
                    Price = 1000
                };
            
            var bookgeneric = new Mock<IGenericRepository<Bookstore>>();
            bookgeneric.Setup(x => x.GetByID(It.IsAny<int>()))
                .Returns(Task.FromResult<Bookstore>(book));

            Mock<IGenericRepository<Order>> ordergeneric = new Mock<IGenericRepository<Order>>();

            _uniOfWorkMock.Setup(bo => bo.BookstoreRepository).Returns(bookgeneric.Object);

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.OrderBookAsync(cart);

            //Assert

            var content = action.Result;

            Assert.IsFalse(content);
            _uniOfWorkMock.Verify(u => u.Save(), Times.Exactly(0));
            _uniOfWorkMock.Verify(u => u.BookstoreRepository.Update(It.IsAny<Bookstore>()), Times.Exactly(0));
            _uniOfWorkMock.Verify(u => u.OrderRepository.Insert(It.IsAny<Order>()), Times.Exactly(0));

        }

        [Test]
        public void SubmitOrder_BookIdNull_ReturnFalse()
        {
            //Arrange
            var cart = new List<Cart>
            {
                new Cart
                {
                    Author = "test Author",
                    Title = "test Title",
                    Number = 7,
                    Price = 2000
                }
            };

            var order = new List<Order>
            {
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    BookId = 1,
                    Number = 2,
                    TotalPrice = 4000
                }
            };

            var book =
                new Bookstore
                {
                    Id = 1,
                    Author = "test Author",
                    Title = "test Title",
                    inStock = 6,
                    Price = 1000
                };

            var bookgeneric = new Mock<IGenericRepository<Bookstore>>();
            bookgeneric.Setup(x => x.GetByID(It.IsAny<int>()))
                .Returns(Task.FromResult<Bookstore>(book));

            Mock<IGenericRepository<Order>> ordergeneric = new Mock<IGenericRepository<Order>>();

            _uniOfWorkMock.Setup(bo => bo.BookstoreRepository).Returns(bookgeneric.Object);

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.OrderBookAsync(cart);

            //Assert

            var content = action.Result;

            Assert.IsFalse(content);
            _uniOfWorkMock.Verify(u => u.Save(), Times.Exactly(0));
            _uniOfWorkMock.Verify(u => u.BookstoreRepository.Update(It.IsAny<Bookstore>()), Times.Exactly(0));
            _uniOfWorkMock.Verify(u => u.OrderRepository.Insert(It.IsAny<Order>()), Times.Exactly(0));
        }

        [Test]
        public void SubmitOrder_CorrectData_ReturnTrue()
        {
            //Arrange
            var cart = new List<Cart>
            {
                new Cart
                {
                    Author = "test Author",
                    Title = "test Title",
                    Number = 2,
                    Price = 2000
                }
            };

            var order = new List<Order>
            {
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    BookId = 1,
                    Number = 2,
                    TotalPrice = 4000
                }
            };

            var book =
                new Bookstore
                {
                    Id = 1,
                    Author = "test Author",
                    Title = "test Title",
                    inStock = 6,
                    Price = 1000
                };


            var ordergeneric = new Mock<IGenericRepository<Order>>();
            var bookgeneric = new Mock<IGenericRepository<Bookstore>>();


            _uniOfWorkMock.Setup(x => x.BookstoreRepository.GetByID(It.IsAny<int>()))
                .Returns(Task.FromResult<Bookstore>(book));
            _uniOfWorkMock.Setup(x => x.OrderRepository.Insert(It.IsAny<Order>())).Returns(Task.FromResult<bool>(true));

            var bookService = new BookService(_uniOfWorkMock.Object);

            //Act
            var action = bookService.OrderBookAsync(cart);

            //Assert

            var content = action.Result;

            Assert.IsTrue(content);
            _uniOfWorkMock.Verify(u => u.Save(), Times.Exactly(1));
            _uniOfWorkMock.Verify(u => u.BookstoreRepository.Update(It.IsAny<Bookstore>()), Times.Exactly(1));
            _uniOfWorkMock.Verify(u => u.OrderRepository.Insert(It.IsAny<Order>()), Times.Exactly(1));
        }
    }
}
