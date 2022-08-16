using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Entities.Models;
using Foresythe.Configurations;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Foresythe.Repositories
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        { 
        }

        public IQueryable<Book> FindBookByTitleAsync(string title)
        {
            var book = FindByCondition(x => x.Title.Equals(title));
            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var books = await FindByCondition(b => b.Id == b.Id)
                .Include(a => a.Authors)
                .ToListAsync(); 

            return books;
        }
        
        public IQueryable<Book> FindBookByISBNAsync(string ISBN)
        {
            var book = FindByCondition(b => b.ISBN.Equals(ISBN));
            return book;
        }

        public IQueryable<Book> GetAllBooksInGenre(string genre)
        {
            var books = FindByCondition(b => b.Genre.Equals(genre));
            return books;
        }

        public async Task<Book> GetBookAsync(Guid BookId)
        {
            var book = await FindByCondition(b => b.Id == BookId)
                .Include(a => a.Authors)
                .SingleOrDefaultAsync();

            return book;
        }
    }
}
