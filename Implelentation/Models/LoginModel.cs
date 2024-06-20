<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;

namespace aqay_apis.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
=======
﻿using System.ComponentModel.DataAnnotations;

namespace aqay_apis.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
>>>>>>> 7076cfbc8c682a2ab0ed0420e7542082677ac640
