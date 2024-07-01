using System.ComponentModel.DataAnnotations;

namespace aqay_apis.Models
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool isSubscriped { get; set; } = false;
    }
}
