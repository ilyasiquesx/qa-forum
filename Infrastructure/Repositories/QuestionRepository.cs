using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QAForum.Application.Common.Interfaces;
using QAForum.Domain.Entities;
using QAForum.Infrastructure.Context;

namespace QAForum.Infrastructure.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _appDbContext;

        public QuestionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Question> GetByIdAsync(long id)
        {
            return await _appDbContext.Questions.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task CreateAsync(Question model)
        {
            _appDbContext.Questions.Add(model);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Question model)
        {
            _appDbContext.Questions.Update(model);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Question model)
        {
            _appDbContext.Questions.Remove(model);
            await _appDbContext.SaveChangesAsync();
        }

        public IQueryable<Question> GetAll()
        {
            return _appDbContext.Questions;
        }

        public async Task<bool> DoesExistAsync(long id)
        {
            return await _appDbContext.Questions.FirstOrDefaultAsync(q => q.Id == id) is not null;
        }
    }
}