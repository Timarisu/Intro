using Microsoft.AspNetCore.Identity;

namespace Intro.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
    }
}
