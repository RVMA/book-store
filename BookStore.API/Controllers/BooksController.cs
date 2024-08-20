using AutoMapper;
using BookStore.API.DTOs;
using BookStore.Business.Models;
using BookStore.Business.Services;
using BookStore.Data.Entities;
using BookStore.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public BooksController(BookService bookService, IMapper mapper, IMemoryCache cache)
        {
            _bookService = bookService;
            _mapper = mapper;
            _cache = cache;
        }

        // GET /api/books
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] QueryParamsDto qParams)
        {
            var cacheKey = $"Books_{qParams.PageNumber}_{qParams.PageSize}_{qParams.Search}";
            if (!_cache.TryGetValue(cacheKey, out PagedData<Book> cachedBooks))
            {
                var books = await _bookService.GetAllBooksAsync(qParams);
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                };
                _cache.Set(cacheKey, books, cacheOptions);
                return Ok(books);
            }
            return Ok(cachedBooks);
        }

        // GET /api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var bookDto = _mapper.Map<BookDto>(book);
            return Ok(bookDto);
        }

        // POST /api/books
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var book = _mapper.Map<Book>(bookDto);
            await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.BookId }, book);
        }

        // PUT /api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book bookDto)
        {
            if (id != bookDto.BookId)
            {
                return BadRequest();
            }
            var book = _mapper.Map<Book>(bookDto);
            await _bookService.UpdateBookAsync(book);
            return NoContent();
        }

        // DELETE /api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}
