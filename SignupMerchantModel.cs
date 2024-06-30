using System.ComponentModel.DataAnnotations;
using aqay_apis.Validations;

namespace aqay_apis.Models
{
    public class SignupMerchantModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordConfirm { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [TaxRegistrationOrNationalIdRequired]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Tax Registration Number must be exactly 9 characters long.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid Tax Registration Number format.")]
        public string? TaxRegistrationNumber { get; set; } = string.Empty;

        [TaxRegistrationOrNationalIdRequired]
        [ValidNationalId]
        public string? NationalId { get; set; } = string.Empty;
    }
}
