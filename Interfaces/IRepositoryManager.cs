using System;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IRepositoryManager
    {
        IBookRepository bookRepository { get; }
        IAuthorRepository authorRepository { get; }
        Task SaveAsync();
    }
}
