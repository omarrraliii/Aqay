<<<<<<< HEAD
﻿namespace aqay_apis.Models
{
    public class Merchant : User
    {
        public Brand? Brand { get; set; }
        public bool IsVerified { get; set; }
        public bool IsOwner { get; set; }
        //one to one relationship
        public Subscription Subscription { get; set; }
    }
}
=======
﻿namespace aqay_apis.Models
{
    public class Merchant : User
    {
        public Brand? Brand { get; set; }
        public bool IsVerified { get; set; }
        public bool IsOwner { get; set; }
    }
}
>>>>>>> 7076cfbc8c682a2ab0ed0420e7542082677ac640
