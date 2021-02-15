using QAForum.Domain.Interfaces;

namespace QAForum.Application.Common.Interfaces
{
    public interface IAccessValidator
    {
        bool HasAccessToModify(string userId, IAuditableEntity entity);
    }
}