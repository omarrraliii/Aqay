using aqay_apis.Models;

namespace aqay_apis.Models
{
    public class Merchant : User
    {
        public bool IsVerified { get; set; } = false;
        public bool IsOwner { get; set; } = true;
        public bool IsSubscriped { get; set; } = false;
        public string role { get; set; } = "Owner";
        public string BrandName { get; set; }
        //Relationships
        public Brand Brand { get; set; }
        
        
    }
}
