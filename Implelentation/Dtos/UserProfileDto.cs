namespace aqay_apis.Dtos
{
    public class UserProfileDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
