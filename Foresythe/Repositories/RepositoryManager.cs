using System;
using System.Threading.Tasks;
using Interfaces;

namespace Foresythe.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IBookRepository bookRepository
        {
            get
            {
                if (_bookRepository == null)
                    _bookRepository = new BookRepository(_repositoryContext);

                return _bookRepository;
            }
        }

        public IAuthorRepository authorRepository
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
