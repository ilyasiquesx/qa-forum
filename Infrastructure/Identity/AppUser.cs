using Microsoft.AspNetCore.Identity;
using QAForum.Domain.Entities;

namespace QAForum.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}