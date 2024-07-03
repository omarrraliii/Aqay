using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace aqay_apis.Models
{
    public class User : IdentityUser
    {
        public bool Gender { get; set; } // 0 -> males 1 -> females

        //set it by default to true
        public bool IsActive { get; set; } = true;
    }
}
