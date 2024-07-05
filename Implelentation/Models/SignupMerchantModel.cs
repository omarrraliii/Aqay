using System.ComponentModel.DataAnnotations;
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
        public string? TaxRegistrationNumber { get; set; }
        public string? NationalId { get; set; }
        public string BrandName { get; set; }
    }
}
