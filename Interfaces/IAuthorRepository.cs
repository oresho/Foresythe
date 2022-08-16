using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;

namespace Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorAsync(Guid AuthorId);
        IQueryable<Author> FindAuthorByName(string name);
        Task CreateAsync(Author author);
        void Update(Author author);
        void Delete(Author author);
    }
}
