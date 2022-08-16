using System;
using System.Threading.Tasks;
using Foresythe.Configurations;
using Interfaces;

namespace Foresythe.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IBookRepository BookRepository
        {
            get
            {
                if (_bookRepository == null)
                    _bookRepository = new BookRepository(_repositoryContext);

                return _bookRepository;
            }
        }

        public IAuthorRepository AuthorRepository
        {
            get
            {
                if (_authorRepository == null)
                    _authorRepository = new AuthorRepository(_repositoryContext);

                return _authorRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
