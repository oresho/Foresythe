using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Foresythe.Repositories
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {

        public AuthorRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IQueryable<Author> FindAuthorByName(string name)
        {
            var author = FindByCondition(a => a.Name.Equals(name))
                .Include(a => a.Books);
            return author;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            var authors = await GetAllAsync();
            return authors;
        }

        public async Task AddNewAuthorAsync(Author author)
        {
            await CreateAsync(author);
        }

        public void DeleteAuthor(Author author)
        {
            Delete(author);
        }

        public void UpdateAuthor(Author author)
        {
            Update(author);
        }

        public async Task<Author> GetAuthorAsync(Guid AuthorId)
        {
            var author = await FindByCondition(a => a.Id == AuthorId)
                .Include(a => a.Books)
                .SingleOrDefaultAsync();

            return author;
        }
    }
}
