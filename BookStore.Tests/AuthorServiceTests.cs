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
    public class AuthorServiceTests
    {
        private readonly Mock<IRepository<Author>> _mockAuthorRepository;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _mockAuthorRepository = new Mock<IRepository<Author>>();
            _authorService = new AuthorService(_mockAuthorRepository.Object);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_ReturnsPagedData()
        {
            var authors = new List<Author>
            {
                new Author { AuthorId = 1, Name = "Author 1" },
                new Author { AuthorId = 2, Name = "Author 2" }
            }.AsQueryable().BuildMock();

            var queryParams = new QueryParamsDto { PageNumber = 1, PageSize = 10, Search = "Author" };

            _mockAuthorRepository.Setup(repo => repo.GetAll()).Returns(authors);

            var result = await _authorService.GetAllAuthorsAsync(queryParams);

            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ReturnsAuthor()
        {
            var author = new Author { AuthorId = 1, Name = "Author 1" };
            _mockAuthorRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(author);
            var result = await _authorService.GetAuthorByIdAsync(1);
            Assert.Equal(author, result);
        }

        [Fact]
        public async Task AddAuthorAsync_AddsAuthor()
        {
            var author = new Author { AuthorId = 1, Name = "Author 1" };
            await _authorService.AddAuthorAsync(author);
            _mockAuthorRepository.Verify(repo => repo.AddAsync(author), Times.Once);
        }

        [Fact]
        public async Task UpdateAuthorAsync_UpdatesAuthor()
        {
            var author = new Author { AuthorId = 1, Name = "Author 1" };
            await _authorService.UpdateAuthorAsync(author);
            _mockAuthorRepository.Verify(repo => repo.UpdateAsync(author), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthorAsync_DeletesAuthor()
        {
            var authorId = 1;
            await _authorService.DeleteAuthorAsync(authorId);
            _mockAuthorRepository.Verify(repo => repo.DeleteAsync(authorId), Times.Once);
        }
    }
}
