using System.Collections.Generic;

namespace QAForum.Application.Common.Models
{
    public class RegisterResult
    {
        public bool IsSucceeded { get; init; }
        public IEnumerable<string> Errors { get; init; } = new List<string>();
    }
}