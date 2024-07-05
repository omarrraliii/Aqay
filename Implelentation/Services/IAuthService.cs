using aqay_apis.Models;

namespace aqay_apis.Services
{
    public interface IAuthService
    {
        Task<string> SignupMerchantAsync(SignupMerchantModel model);
        Task<AuthModel> SignupConsumerAsync(SignupConsumerModel model);

        // Log in = Get Token (fetsh the token to allow authentication)
        Task<AuthModel> LoginAsync(LoginModel model);
        Task<string> CreateAdminAsync(string email, string password);
    }
}
