using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAForum.Domain.Entities;

namespace QAForum.Application.Common.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        IQueryable<Question> GetAll();
        Task<bool> DoesExistAsync(long id);
    }
}