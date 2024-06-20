<<<<<<< HEAD
﻿namespace aqay_apis.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string Balance { get; set; }
        public string UserId { get; set; } // Foreign key
        public User User { get; set; }
    }
}
=======
﻿namespace aqay_apis.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string Balance { get; set; }
        public string UserId { get; set; } // Foreign key
        public User User { get; set; }
    }
}
>>>>>>> 7076cfbc8c682a2ab0ed0420e7542082677ac640
