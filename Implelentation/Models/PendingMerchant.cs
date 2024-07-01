namespace aqay_apis.Models
{
    public class PendingMerchant
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string NATID { get; set; }
        public string? TRN { get; set; }
    }
}
