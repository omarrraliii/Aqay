using aqay_apis.Models;

namespace aqay_apis.Services
{
    public interface IAuthService
    {
        // sign up as a brand owener
        Task<string> SignupMerchantAsync(SignupMerchantModel model);
        // sign up as a consumer
        Task<AuthModel> SignupConsumerAsync(SignupConsumerModel model);

        // Log in = Get Token (fetsh the token to allow authentication)
        Task<AuthModel> LoginAsync(LoginModel model);
    }
}
