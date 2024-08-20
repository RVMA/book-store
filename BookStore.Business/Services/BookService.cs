using BookStore.Business.Models;
using BookStore.Data.Entities;
using BookStore.Data.Repositories;
using BookStore.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class BookService
    {
        private readonly IRepository<Book> _bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<PagedData<Book>> GetAllBooksAsync(QueryParamsDto qParams)
        {
            var query = _bookRepository.GetAll();
            if (!string.IsNullOrEmpty(qParams.Search))
            {
                query = query.Where(b => b.Title.Contains(qParams.Search));
            }
            int totalCount = await query.CountAsync();
            var items = await query.Skip((qParams.PageNumber - 1) * qParams.PageSize)
                .Take(qParams.PageSize)
                .ToListAsync();
            return new PagedData<Book>
            {
                Data = items,
                TotalCount = totalCount,
                PageNumber = qParams.PageNumber,
                PageSize = qParams.PageSize
            };
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteAsync(id);
        }
    }
}
