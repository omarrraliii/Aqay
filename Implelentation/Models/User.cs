<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace aqay_apis.Models
{
    public class User : IdentityUser
    {
        public bool Gender { get; set; } // 0 -> males 1 -> females
        public int Age { get; set; }

        //set it by default to true
        public bool IsActive { get; set; } = true;
        // Each user has one wallet (one-to-one)
        public Wallet Wallet { get; set; }
    }
}
=======
﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace aqay_apis.Models
{
    public class User : IdentityUser
    {
        public bool Gender { get; set; } // 0 -> males 1 -> females
        public int Age { get; set; }

        //set it by default to true
        public bool IsActive { get; set; } = true;
        // Each user has one wallet (one-to-one)
        public Wallet Wallet { get; set; }
    }
}
>>>>>>> 7076cfbc8c682a2ab0ed0420e7542082677ac640
