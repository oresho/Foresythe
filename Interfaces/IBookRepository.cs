using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;

namespace Interfaces
{
    public interface IBookRepository 
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookAsync(Guid BookId);
        IQueryable<Book> FindBookByTitleAsync(string title);
        Task CreateAsync(Book book);
        void Update(Book book);
        void Delete(Book book);

        IQueryable<Book> FindBookByISBNAsync(string ISBN);
        IQueryable<Book> GetAllBooksInGenre(string genre);
    }
}
