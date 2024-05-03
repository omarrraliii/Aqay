namespace aqay_apis.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string Balance { get; set; }
        public string UserId { get; set; } // Foreign key
        public User User { get; set; }
    }
}
