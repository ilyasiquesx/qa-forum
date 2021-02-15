using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QAForum.Application.Common.Interfaces;
using QAForum.Domain.Entities;
using QAForum.Infrastructure.Context;

namespace QAForum.Infrastructure.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _appDbContext;

        public AnswerRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Answer> GetByIdAsync(long id)
        {
            return await _appDbContext.Answers.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CreateAsync(Answer model)
        {
            _appDbContext.Answers.Add(model);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Answer model)
        {
            _appDbContext.Answers.Update(model);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Answer model)
        {
            _appDbContext.Answers.Remove(model);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> DoesAnswerExistAsync(long id)
        {
            return (await _appDbContext.Answers.FirstOrDefaultAsync(a => a.Id == id)) is not null;
        }
    }
}