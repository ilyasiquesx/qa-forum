using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QAForum.Domain.Interfaces;

namespace QAForum.Domain.Entities
{
    public class Question : IHasId<long>, IAuditableEntity
    {
        public long Id { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public virtual User Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        [MaxLength(128)]
        public string Title { get; set; }

        [MaxLength(2048)]
        public string Content { get; set; }

        public virtual Answer BestAnswer { get; set; }
        
        public long? BestAnswerId { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}