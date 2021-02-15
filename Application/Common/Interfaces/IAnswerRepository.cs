using System.Threading.Tasks;
using QAForum.Domain.Entities;

namespace QAForum.Application.Common.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<bool> DoesAnswerExistAsync(long id);
    }
}