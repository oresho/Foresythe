using System;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IRepositoryManager
    {
        IBookRepository BookRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        Task SaveAsync();
    }
}
