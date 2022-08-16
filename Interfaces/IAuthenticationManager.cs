using System.Threading.Tasks;
using Entities.DTOs;

namespace Interfaces
{
    public interface IAuthenticationManager
    { 
        Task<string> CreateToken();
    }
}
