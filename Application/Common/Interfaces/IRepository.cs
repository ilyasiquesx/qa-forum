using System.Threading.Tasks;

namespace QAForum.Application.Common.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(long id);
        Task CreateAsync(T model);
        Task UpdateAsync(T model);
        Task DeleteAsync(T model);
    }
}