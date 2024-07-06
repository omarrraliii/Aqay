namespace aqay_apis.Models
{
    public class UpdateUserDto
    {
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
