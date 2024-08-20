using BookStore.Business.Models;
using BookStore.Business.Services;
using BookStore.Data.Entities;
using BookStore.Data.Repositories;
using BookStore.Shared.DTOs;
using Moq;
using MockQueryable.Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests
{
    public class BookServiceTest
    {
        private readonly Mock<IRepository<Book>> _mockBookRepository;
        private readonly BookService _bookService;
        private readonly Author _author;

        public BookServiceTest()
        {
            _mockBookRepository = new Mock<IRepository<Book>>();
            _bookService = new BookService(_mockBookRepository.Object);
            _author = new Author { AuthorId = 1, Name = "Author 1" };
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsPagedData()
        {
            var books = new List<Book>
            {
                new Book { BookId = 1, Title = "Book 1", Genre = "Genre 1", PublicationDate = DateTime.Now, AuthorId = _author.AuthorId, Author = _author },
                new Book { BookId = 2, Title = "Book 2", Genre = "Genre 2", PublicationDate = DateTime.Now, AuthorId = _author.AuthorId, Author = _author }
            }.AsQueryable().BuildMock();

            var queryParams = new QueryParamsDto { PageNumber = 1, PageSize = 10, Search = "Book" };

            _mockBookRepository.Setup(repo => repo.GetAll()).Returns(books);

            var result = await _bookService.GetAllBooksAsync(queryParams);

            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsBook()
        {
            var book = new Book { BookId = 1, Title = "Book 1", Genre = "Genre 1", PublicationDate = DateTime.Now, AuthorId = _author.AuthorId, Author = _author };
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);

            var result = await _bookService.GetBookByIdAsync(1);

            Assert.Equal(book, result);
        }

        [Fact]
        public async Task AddBookAsync_AddsBook()
        {
            var book = new Book { BookId = 1, Title = "Book 1", Genre = "Genre 1", PublicationDate = DateTime.Now, AuthorId = _author.AuthorId, Author = _author };

            await _bookService.AddBookAsync(book);

            _mockBookRepository.Verify(repo => repo.AddAsync(book), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_UpdatesBook()
        {
            var book = new Book { BookId = 1, Title = "Book 1", Genre = "Genre 1", PublicationDate = DateTime.Now, AuthorId = _author.AuthorId, Author = _author };

            await _bookService.UpdateBookAsync(book);

            _mockBookRepository.Verify(repo => repo.UpdateAsync(book), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_DeletesBook()
        {
            var bookId = 1;

            await _bookService.DeleteBookAsync(bookId);

            _mockBookRepository.Verify(repo => repo.DeleteAsync(bookId), Times.Once);
        }
    }
}
