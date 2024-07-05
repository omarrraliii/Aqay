using Microsoft.AspNetCore.Identity;
namespace aqay_apis.Models
{
    public class User : IdentityUser
    {
        public bool Gender { get; set; } // 0 -> males 1 -> females
        public bool IsActive { get; set; } = true;
    }
}
