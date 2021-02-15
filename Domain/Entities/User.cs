using System.Collections.Generic;
using QAForum.Domain.Interfaces;

namespace QAForum.Domain.Entities
{
    public class User : IHasId<string>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}