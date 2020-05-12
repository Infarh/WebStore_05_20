using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";
        public const string AdminDefaultPassword = "AdminPassword";
    }
}
