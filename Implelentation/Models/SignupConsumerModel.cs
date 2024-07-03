using System.ComponentModel.DataAnnotations;

namespace aqay_apis.Models
{
    public class SignupConsumerModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordConfirm { get; set; }
        [Required]
        public bool Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
