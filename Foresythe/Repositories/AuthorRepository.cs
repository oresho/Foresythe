using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Foresythe.Configurations;
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
        
        public async Task<Author> GetAuthorAsync(Guid AuthorId)
        {
            var author = await FindByCondition(a => a.Id == AuthorId)
                .Include(a => a.Books)
                .SingleOrDefaultAsync();

            return author;
        }
    }
}
