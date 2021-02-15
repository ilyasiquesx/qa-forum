using System.Threading.Tasks;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<RegisterResult> CreateUserAsync(User user, string password);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}