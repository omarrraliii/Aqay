namespace aqay_apis.Models
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public DateOnly dateOfBirth { get; set; }
    }
}
