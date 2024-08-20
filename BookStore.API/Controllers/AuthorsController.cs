using AutoMapper;
using BookStore.API.DTOs;
using BookStore.Business.Models;
using BookStore.Business.Services;
using BookStore.Data.Entities;
using BookStore.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _authorService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public AuthorsController(AuthorService authorService, IMapper mapper, IMemoryCache cache)
        {
            _authorService = authorService;
            _mapper = mapper;
            _cache = cache;
        }

        // GET /api/authors
        [HttpGet]
        public async Task<IActionResult> GetAuthors([FromQuery] QueryParamsDto qParams)
        {
            var cacheKey = $"Authors_{qParams.PageNumber}_{qParams.PageSize}_{qParams.Search}";
            if (!_cache.TryGetValue(cacheKey, out PagedData<Author> cachedAuthors))
            {
                var authors = await _authorService.GetAllAuthorsAsync(qParams);
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                };
                _cache.Set(cacheKey, authors, cacheOptions);
                return Ok(authors);
            }
            return Ok(cachedAuthors);
        }

        // GET /api/authors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            var authorDto = _mapper.Map<AuthorDto>(author);
            return Ok(authorDto);
        }

        // POST /api/authors
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var author = _mapper.Map<Author>(authorDto);
            await _authorService.AddAuthorAsync(author);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.AuthorId }, author);
        }

        // PUT /api/authors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorDto authorDto)
        {
            if (id != authorDto.AuthorId)
            {
                return BadRequest();
            }
            var author = _mapper.Map<Author>(authorDto);

            await _authorService.UpdateAuthorAsync(author);
            return NoContent();
        }

        // DELETE /api/authors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorService.DeleteAuthorAsync(id);
            return NoContent();
        }
    }
}
