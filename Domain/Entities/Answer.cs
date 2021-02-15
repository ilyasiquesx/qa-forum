using System;
using System.ComponentModel.DataAnnotations;
using QAForum.Domain.Interfaces;

namespace QAForum.Domain.Entities
{
    public class Answer : IHasId<long>, IAuditableEntity
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual User Created { get; set; }

        public string CreatedBy { get; set; }

        public virtual Question Question { get; set; }
        
        public long QuestionId { get; set; }

        [MaxLength(2048)] 
        public string Content { get; set; }
    }
}