using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Aqay_v2.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required ]
        public bool is_active { get; set; } = true;
        public string? Address { get; set; }
        public string City { get; set; } = "Giza";
        public string? Street { get; set; }
        public int Building { get; set; }
        public int Floor { get; set; }
        public int Apartment { get; set; }
    }
}
