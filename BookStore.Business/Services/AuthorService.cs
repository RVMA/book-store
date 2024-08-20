using BookStore.Business.Models;
using BookStore.Data.Entities;
using BookStore.Data.Repositories;
using BookStore.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class AuthorService
    {
        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<PagedData<Author>> GetAllAuthorsAsync(QueryParamsDto qParams)
        {
            var query = _authorRepository.GetAll();
            if (!string.IsNullOrEmpty(qParams.Search))
            {
                query = query.Where(b => b.Name.Contains(qParams.Search));
            }
            int totalCount = await query.CountAsync();
            var items = await query.Skip((qParams.PageNumber - 1) * qParams.PageSize)
                .Take(qParams.PageSize)
                .ToListAsync();
            return new PagedData<Author>
            {
                Data = items,
                TotalCount = totalCount,
                PageNumber = qParams.PageNumber,
                PageSize = qParams.PageSize
            };
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await _authorRepository.GetByIdAsync(id);
        }

        public async Task AddAuthorAsync(Author author)
        {
            await _authorRepository.AddAsync(author);
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            await _authorRepository.UpdateAsync(author);
        }

        public async Task DeleteAuthorAsync(int id)
        {
            await _authorRepository.DeleteAsync(id);
        }
    }
}
