<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;

namespace aqay_apis.Models
{
    public class SignupConsumerModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordConfirm { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public int Day { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }
    }
}
=======
﻿using System.ComponentModel.DataAnnotations;

namespace aqay_apis.Models
{
    public class SignupConsumerModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordConfirm { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public int Day { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }
    }
}
>>>>>>> 7076cfbc8c682a2ab0ed0420e7542082677ac640
