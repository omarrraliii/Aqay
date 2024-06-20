namespace aqay_apis.Models
{
    public class Merchant : User
    {
        public Brand? Brand { get; set; }
        public bool IsVerified { get; set; }
        public bool IsOwner { get; set; }
    }
}
