using QAForum.Application.Common.Interfaces;
using QAForum.Domain.Interfaces;

namespace QAForum.Application.Common.Services
{
    public class AccessValidator : IAccessValidator
    {
        public bool HasAccessToModify(string userId, IAuditableEntity entity)
        {
            return entity.CreatedBy == userId;
        }
    }
}