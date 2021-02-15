using System.Collections.Generic;

namespace QAForum.Application.Common.Models
{
    public class Response<T>
    {
        public bool IsSucceeded { get; private set; }
        public ErrorReason? ErrorReason { get; set; }
        public IDictionary<string, IList<string>> Errors { get; set; } = new Dictionary<string, IList<string>>();
        public T Data { get; private set; }

        public void SetData(T data = default)
        {
            IsSucceeded = true;
            Data = data;
        }
    }

    public enum ErrorReason
    {
        AlreadyExist = 0,
        NotFound = 1,
        WrongCredentials = 2,
        HaveNoAccess = 3,
        InvalidData = 4
    }
}